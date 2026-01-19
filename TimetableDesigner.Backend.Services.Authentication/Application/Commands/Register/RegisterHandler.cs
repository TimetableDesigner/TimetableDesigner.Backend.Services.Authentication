using MediatR;
using TimetableDesigner.Backend.Events;
using TimetableDesigner.Backend.Services.Authentication.Application.Helpers;
using TimetableDesigner.Backend.Services.Authentication.Database;
using TimetableDesigner.Backend.Services.Authentication.Database.Model;
using TimetableDesigner.Backend.Services.Authentication.DTO.Events;

namespace TimetableDesigner.Backend.Services.Authentication.Application.Commands.Register;

public class RegisterHandler : IRequestHandler<RegisterCommand, RegisterResult>
{
    private readonly DatabaseContext _databaseContext;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IEventQueuePublisher _eventQueuePublisher;
    
    public RegisterHandler(DatabaseContext databaseContext, IPasswordHasher passwordHasher, IEventQueuePublisher eventQueuePublisher)
    {
        _databaseContext = databaseContext;
        _passwordHasher = passwordHasher;
        _eventQueuePublisher = eventQueuePublisher;
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
        await _databaseContext.SaveChangesAsync(cancellationToken);

        RegisterEvent eventData = account.ToEvent();
        await _eventQueuePublisher.PublishAsync(eventData);

        RegisterResult result = account.ToResult();
        return result;
    }
}