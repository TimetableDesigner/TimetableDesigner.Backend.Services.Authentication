using MediatR;
using TimetableDesigner.Backend.Services.Authentication.Application.Helpers;
using TimetableDesigner.Backend.Services.Authentication.Database;

namespace TimetableDesigner.Backend.Services.Authentication.Application.Commands.Register;

public class RegisterHandler : IRequestHandler<RegisterCommand, RegisterData>
{
    private readonly DatabaseContext _databaseContext;
    private readonly IPasswordHasher _passwordHasher;
    
    public RegisterHandler(DatabaseContext databaseContext, IPasswordHasher passwordHasher)
    {
        _databaseContext = databaseContext;
        _passwordHasher = passwordHasher;
    }

    public async Task<RegisterData> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        PasswordHashData hash = _passwordHasher.CreateHash(command.Password);
        
    }
}