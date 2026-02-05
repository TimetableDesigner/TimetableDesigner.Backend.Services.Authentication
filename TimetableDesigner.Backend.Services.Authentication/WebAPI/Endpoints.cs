using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using TimetableDesigner.Backend.Services.Authentication.Core.Commands.AuthPassword;
using TimetableDesigner.Backend.Services.Authentication.Core.Commands.AuthToken;
using TimetableDesigner.Backend.Services.Authentication.Core.Commands.Register;
using TimetableDesigner.Backend.Services.Authentication.DTO.WebAPI;
using TimetableDesigner.Backend.Services.Authentication.WebAPI.Mappers;

namespace TimetableDesigner.Backend.Services.Authentication.WebAPI;

public static class Endpoints
{
    public static IEndpointRouteBuilder MapWebAPIEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("/register", Register)
               .AllowAnonymous()
               .Produces<RegisterResponse>(201)
               .Produces<HttpValidationProblemDetails>(400)
               .Produces(500)
               .WithName("Register");
        builder.MapPost("/auth/password", AuthPassword)
               .AllowAnonymous()
               .Produces<AuthResponse>()
               .Produces(401)
               .Produces(500)
               .WithName("AuthPassword");
        builder.MapPost("/auth/token", AuthToken)
               .AllowAnonymous()
               .Produces<AuthResponse>()
               .Produces(401)
               .Produces(500)
               .WithName("AuthToken");

        return builder;
    }

    private static async Task<Results<Created<RegisterResponse>, ValidationProblem, InternalServerError>> Register(IMediator mediator, IValidator<RegisterRequest> validator, RegisterRequest request, CancellationToken cancellationToken)
    {
        ValidationResult validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }
        
        RegisterResult result = await mediator.Send(request.ToCommand(), cancellationToken);
        
        RegisterResponse response = result.ToResponse();
        return TypedResults.Created($"accounts/{response.Id}", response);
    }

    private static async Task<Results<Ok<AuthResponse>, UnauthorizedHttpResult, InternalServerError>> AuthPassword(IMediator mediator, AuthPasswordRequest request, CancellationToken cancellationToken)
    {
        AuthPasswordResult result = await mediator.Send(request.ToCommand(), cancellationToken);

        if (!result.IsSuccess)
        {
            return TypedResults.Unauthorized();
        }
        
        AuthResponse response = result.ToResponse();
        return TypedResults.Ok(response);
    }
    
    private static async Task<Results<Ok<AuthResponse>, UnauthorizedHttpResult, InternalServerError>> AuthToken(IMediator mediator, AuthTokenRequest request, CancellationToken cancellationToken)
    {
        AuthTokenResult result = await mediator.Send(request.ToCommand(), cancellationToken);

        if (!result.IsSuccess)
        {
            return TypedResults.Unauthorized();
        }
        
        AuthResponse response = result.ToResponse();
        return TypedResults.Ok(response);
    }
}