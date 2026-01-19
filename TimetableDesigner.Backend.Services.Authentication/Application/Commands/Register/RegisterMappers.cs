using TimetableDesigner.Backend.Services.Authentication.Database.Model;
using TimetableDesigner.Backend.Services.Authentication.DTO.Events;
using TimetableDesigner.Backend.Services.Authentication.DTO.WebAPI;

namespace TimetableDesigner.Backend.Services.Authentication.Application.Commands.Register;

public static class RegisterMappers
{
    public static RegisterCommand ToCommand(this RegisterRequest request) =>
        new RegisterCommand(request.Email, request.Password);
    
    public static RegisterResult ToResult(this Account account) => 
        new RegisterResult(account.Id, account.Email);
    
    public static RegisterResponse ToResponse(this RegisterResult result) =>
        new RegisterResponse(result.Id, result.Email);
    
    public static RegisterEvent ToEvent(this Account account) =>
        new RegisterEvent(account.Id, account.Email);
}