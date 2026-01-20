using TimetableDesigner.Backend.Services.Authentication.Core.Commands.Register;
using TimetableDesigner.Backend.Services.Authentication.DTO.WebAPI;

namespace TimetableDesigner.Backend.Services.Authentication.WebAPI.Mappers;

public static class RegisterMappers
{
    public static RegisterCommand ToCommand(this RegisterRequest request) =>
        new RegisterCommand(request.Email, request.Password);

    public static RegisterResponse ToResponse(this RegisterResult result) =>
        new RegisterResponse(result.Id, result.Email);
}