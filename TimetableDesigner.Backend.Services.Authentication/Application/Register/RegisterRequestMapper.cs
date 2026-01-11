using TimetableDesigner.Backend.Services.Authentication.DTO;

namespace TimetableDesigner.Backend.Services.Authentication.Application.Register;

public static class RegisterRequestMapper
{
    public static RegisterCommand ToCommand(this RegisterRequest request) => new RegisterCommand(request.Email, request.Password);
}