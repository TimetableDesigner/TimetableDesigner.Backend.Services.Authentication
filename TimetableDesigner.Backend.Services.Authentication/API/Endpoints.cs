using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TimetableDesigner.Backend.Services.Authentication.Application.Commands.Register;
using TimetableDesigner.Backend.Services.Authentication.DTO.API;

namespace TimetableDesigner.Backend.Services.Authentication.API;

public static class Endpoints
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/register", Register)
           .AllowAnonymous()
           .Produces<RegisterResponse>(201)
           .Produces<HttpValidationProblemDetails>(400)
           .Produces(500)
           .WithName("Register");
        app.MapPost("/authenticate_password", AuthenticatePassword)
           .WithName("AuthenticatePassword");
        app.MapPost("/authenticate_token", AuthenticateToken)
           .WithName("AuthenticateToken");

        return app;
    }

    private static async Task<Results<Created<RegisterResponse>, ValidationProblem>> Register(IMediator mediator, IValidator<RegisterRequest> validator, RegisterRequest request)
    {
        ValidationResult validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid) 
        {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }
        
        RegisterCommand registerCommand = request.ToCommand();
        RegisterResult result = await mediator.Send(registerCommand);
        RegisterResponse response = result.ToResponse();
        
        return TypedResults.Created($"accounts/{response.Id}", response);
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