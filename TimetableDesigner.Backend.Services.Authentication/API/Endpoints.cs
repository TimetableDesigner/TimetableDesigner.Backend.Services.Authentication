using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using TimetableDesigner.Backend.Services.Authentication.Application.Commands.Register;
using TimetableDesigner.Backend.Services.Authentication.DTO.API;

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

    public static async Task<Results<Created<RegisterResponse>, InternalServerError>> Register(RegisterRequest request, CancellationToken cancellationToken, IMediator mediator)
    {
        RegisterCommand registerCommand = request.ToCommand();
        RegisterData data = await mediator.Send(registerCommand, cancellationToken);
        return Results.Created<RegisterResponse>($"accounts/0", null);
    }

    public static async Task<Results<Ok<AuthenticateResponse>, ProblemHttpResult>> AuthenticatePassword(AuthenticatePasswordRequest request, CancellationToken cancellationToken)
    {
        return null;
    }
    
    public static async Task<Results<Ok<AuthenticateResponse>, ProblemHttpResult>> AuthenticateToken(AuthenticateTokenRequest request, CancellationToken cancellationToken)
    {
        return null;
    }
}