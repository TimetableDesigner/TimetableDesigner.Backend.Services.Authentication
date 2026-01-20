using MediatR;

namespace TimetableDesigner.Backend.Services.Authentication.Core.Commands.Register;

public record RegisterCommand
(
    string Email, 
    string Password
) 
: IRequest<RegisterResult>;