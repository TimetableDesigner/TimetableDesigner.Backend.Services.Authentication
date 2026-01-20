using TimetableDesigner.Backend.Services.Authentication.Core.Commands.AuthPassword;
using TimetableDesigner.Backend.Services.Authentication.DTO.WebAPI;

namespace TimetableDesigner.Backend.Services.Authentication.WebAPI.Mappers;

public static class AuthPasswordMappers
{
    public static AuthPasswordCommand ToCommand(this AuthPasswordRequest request) =>
        new AuthPasswordCommand(request.Email, request.Password, request.RememberMe);

    public static AuthResponse ToResponse(this AuthPasswordResult result) =>
        new AuthResponse(result.AccessToken!, result.RefreshToken!);
}