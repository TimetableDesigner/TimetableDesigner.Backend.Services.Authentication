using System.Reflection;
using FluentValidation;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using TimetableDesigner.Backend.Events.Extensions.AspNetCore.OpenApi;
using TimetableDesigner.Backend.Events.OutboxPattern;
using TimetableDesigner.Backend.Events.Providers.RabbitMQ;
using TimetableDesigner.Backend.Services.Authentication.API;
using TimetableDesigner.Backend.Services.Authentication.API.Validators;
using TimetableDesigner.Backend.Services.Authentication.Core.Helpers;
using TimetableDesigner.Backend.Services.Authentication.Database;
using TimetableDesigner.Backend.Services.Authentication.Events;
using TimetableDesigner.Backend.Services.Authentication.WebAPI;

namespace TimetableDesigner.Backend.Services.Authentication;

public static class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        string databaseConnectionString = builder.Configuration.GetConnectionString("Database")!;
        string eventQueueConnectionString = builder.Configuration.GetConnectionString("EventQueue")!;
        
        builder.Services.AddOpenApi();
        builder.Services.AddDbContext<DatabaseContext>(x => x.UseNpgsql(databaseConnectionString), ServiceLifetime.Transient);
        builder.Services.AddEventQueue<RabbitMQEventQueue>(eventQueueConnectionString);
        builder.Services.AddHelpers();
        builder.Services.AddValidators();
        builder.Services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(Program).Assembly));
        builder.Services.AddWorkers();
        
        WebApplication app = builder.Build();
        
        if (app.Environment.IsDevelopment())
            app.MapOpenApi();
        app.InitializeDatabase();
        //app.UseHttpsRedirection();
        app.MapWebAPIEndpoints();
        app.MapEventHandlers();
        
        app.Run();
    }
    
    private static IServiceCollection AddWorkers(this IServiceCollection services)
    {
        services.AddHostedService<EventOutboxSender<DatabaseContext>>();
        return services;
    }
    
    private static IServiceCollection AddHelpers(this IServiceCollection services)
    {
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddScoped<ITokenGenerator, TokenGenerator>();
        return services;
    }

    private static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<TimetableDesigner.Backend.Services.Authentication.DTO.WebAPI.RegisterRequest>, RegisterRequestValidator>();
        return services;
    }
    
    private static WebApplication InitializeDatabase(this WebApplication app)
    {
        using (IServiceScope scope = app.Services.CreateScope())
        {
            DatabaseContext database = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            while (!database.Database.CanConnect())
            {
                app.Logger.LogInformation("Waiting for database...");
                Thread.Sleep(1000);
            }
            
            database.Database.Migrate();
        }
        return app;
    }
}