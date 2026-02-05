using TimetableDesigner.Backend.Services.Authentication.Core.Commands.AuthToken;
using TimetableDesigner.Backend.Services.Authentication.DTO.WebAPI;

namespace TimetableDesigner.Backend.Services.Authentication.WebAPI.Mappers;

public static class AuthTokenMappers
{
    public static AuthTokenCommand ToCommand(this AuthTokenRequest request) =>
        new AuthTokenCommand(request.AccessToken, request.RefreshToken);

    public static AuthResponse ToResponse(this AuthTokenResult result) =>
        new AuthResponse(result.AccessToken!, result.RefreshToken!);
}