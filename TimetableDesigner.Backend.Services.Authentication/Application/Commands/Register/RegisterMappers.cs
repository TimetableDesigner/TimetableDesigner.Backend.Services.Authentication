using TimetableDesigner.Backend.Services.Authentication.Application.Commands.Register;
using TimetableDesigner.Backend.Services.Authentication.DTO.API;

namespace TimetableDesigner.Backend.Services.Authentication.DTO.Mappers;

public static class RegisterMappers
{
    public static RegisterCommand ToCommand(this RegisterRequest request) =>
        new RegisterCommand(request.Email, request.Password);
}