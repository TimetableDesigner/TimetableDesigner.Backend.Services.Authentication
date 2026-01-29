using MediatR;
using Microsoft.EntityFrameworkCore.Storage;
using TimetableDesigner.Backend.Events.OutboxPattern;
using TimetableDesigner.Backend.Services.Authentication.Core.Helpers;
using TimetableDesigner.Backend.Services.Authentication.Database;
using TimetableDesigner.Backend.Services.Authentication.Database.Model;
using TimetableDesigner.Backend.Services.Authentication.DTO.Events;

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
        
        await using (IDbContextTransaction transaction = await _databaseContext.Database.BeginTransactionAsync(cancellationToken))
        {
            await _databaseContext.Accounts.AddAsync(account, cancellationToken);
            await _databaseContext.SaveChangesAsync(cancellationToken);
            
            Event eventData = Event.Create(new RegisterEvent(account.Id, account.Email));
            await _databaseContext.Events.AddAsync(eventData, cancellationToken);
            await _databaseContext.SaveChangesAsync(cancellationToken);
            
            await transaction.CommitAsync(cancellationToken);
        }

        return new RegisterResult(account.Id, account.Email);
    }
}