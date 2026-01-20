using MediatR;

namespace TimetableDesigner.Backend.Services.Authentication.Core.Commands.AuthPassword;

public record AuthPasswordCommand
(
    string Email,
    string Password,
    bool RememberMe
)
: IRequest<AuthPasswordResult>;