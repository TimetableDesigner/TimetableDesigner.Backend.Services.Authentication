using Microsoft.AspNetCore.Http.HttpResults;
using TimetableDesigner.Backend.Services.Authentication.Application.Register;
using TimetableDesigner.Backend.Services.Authentication.DTO;

namespace TimetableDesigner.Backend.Services.Authentication.API;

public static class Endpoints
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/register", Register)
           .WithName("Register");
        app.MapPost("/authenticate_password", AuthenticatePassword)
           .WithName("AuthenticatePassword");
        app.MapPost("/authenticate_token", AuthenticateToken)
           .WithName("AuthenticateToken");

        return app;
    }

    public static async Task<Results<Created<RegisterResponse>, ProblemHttpResult>> Register(RegisterRequest request, RegisterHandler handler)
    {
        RegisterCommand command = request.ToCommand();
        RegisterResponse account = await handler.HandleAsync(command);
        return Results.Created($"accounts/{account.Id}", account);
    }

    public static async Task<Results<Ok<AuthenticateResponse>, ProblemHttpResult>> AuthenticatePassword(AuthenticatePasswordRequest request)
    {
        return null;
    }
    
    public static async Task<Results<Ok<AuthenticateResponse>, ProblemHttpResult>> AuthenticateToken(AuthenticateTokenRequest request)
    {
        return null;
    }
}