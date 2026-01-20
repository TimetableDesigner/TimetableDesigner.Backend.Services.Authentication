using MediatR;
using TimetableDesigner.Backend.Services.Authentication.Core.Helpers;
using TimetableDesigner.Backend.Services.Authentication.Database;
using TimetableDesigner.Backend.Services.Authentication.Database.Model;

namespace TimetableDesigner.Backend.Services.Authentication.Core.Commands.Register;

public class RegisterHandler : IRequestHandler<RegisterCommand, RegisterResult>
{
    private readonly DatabaseContext _databaseContext;
    private readonly IPasswordHasher _passwordHasher;
    
    public RegisterHandler(DatabaseContext databaseContext, IPasswordHasher passwordHasher)
    {
        _databaseContext = databaseContext;
        _passwordHasher = passwordHasher;
    }

    public async Task<RegisterResult> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        PasswordHashData hash = _passwordHasher.CreateHash(command.Password);
        
        Account account = new Account
        {
            Email = command.Email,
            Password = hash.Hash,
            PasswordSalt = hash.Salt,
        };
        await _databaseContext.Accounts.AddAsync(account, cancellationToken);

        // Change to outbox pattern
        //RegisterEvent eventData = account.ToEvent();
        //await _eventQueuePublisher.PublishAsync(eventData);
        
        await _databaseContext.SaveChangesAsync(cancellationToken);

        return new RegisterResult(account.Id, account.Email);
    }
}