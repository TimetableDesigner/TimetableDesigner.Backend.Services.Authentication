using MediatR;

namespace TimetableDesigner.Backend.Services.Authentication.Core.Commands.AuthToken;

public record AuthTokenCommand
(
    string AccessToken,
    string RefreshToken
) 
: IRequest<AuthTokenResult>;