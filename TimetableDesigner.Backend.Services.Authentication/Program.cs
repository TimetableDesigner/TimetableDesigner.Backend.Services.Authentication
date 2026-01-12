using System.Reflection;
using FluentValidation;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using TimetableDesigner.Backend.Services.Authentication.API;
using TimetableDesigner.Backend.Services.Authentication.Database;
using TimetableDesigner.Backend.Services.Authentication.DTO.Validators;

namespace TimetableDesigner.Backend.Services.Authentication;

public static class Program
{
    public static void Main(string[] args)
    {
        WebApplication app = WebApplication.CreateBuilder(args)
                                           .SetupOpenApi()
                                           .SetupSecurity()
                                           .SetupDatabase()
                                           .SetupValidation()
                                           .Build();
        
        if (app.Environment.IsDevelopment())
            app.MapOpenApi();
        app.InitializeDatabase();
        app.UseHttpsRedirection();
        app.MapEndpoints();
        
        app.Run();
    }

    private static WebApplicationBuilder SetupOpenApi(this WebApplicationBuilder builder)
    {
        builder.Services.AddOpenApi();
        return builder;
    }
    
    private static WebApplicationBuilder SetupSecurity(this WebApplicationBuilder builder)
    {
        //builder.Services.AddAuthorization();
        return builder;
    }

    private static WebApplicationBuilder SetupDatabase(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<DatabaseContext>(x => x.UseNpgsql(builder.Configuration.GetConnectionString("Database")), ServiceLifetime.Transient);
        return builder;
    }

    private static WebApplicationBuilder SetupValidation(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IValidator<TimetableDesigner.Backend.Services.Authentication.DTO.API.RegisterRequest>, RegisterRequestValidator>();
        return builder;
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