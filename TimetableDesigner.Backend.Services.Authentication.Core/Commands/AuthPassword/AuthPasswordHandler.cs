using MediatR;
using Microsoft.EntityFrameworkCore;
using TimetableDesigner.Backend.Services.Authentication.Core.Helpers;
using TimetableDesigner.Backend.Services.Authentication.Database;
using TimetableDesigner.Backend.Services.Authentication.Database.Model;

namespace TimetableDesigner.Backend.Services.Authentication.Core.Commands.AuthPassword;

public class AuthPasswordHandler : IRequestHandler<AuthPasswordCommand, AuthPasswordResult>
{
    private readonly DatabaseContext _databaseContext;
    private readonly IPasswordHasher _passwordHasher;

    public AuthPasswordHandler(DatabaseContext databaseContext, IPasswordHasher passwordHasher)
    {
        _databaseContext = databaseContext;
        _passwordHasher = passwordHasher;
    }
    
    public async Task<AuthPasswordResult> Handle(AuthPasswordCommand request, CancellationToken cancellationToken)
    {
        Account? account = await _databaseContext.Accounts.FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);
        if (account is null)
        {
            return AuthPasswordResult.Failure();
        }

        PasswordHashData hash = new PasswordHashData(account.Password, account.PasswordSalt);
        if (!_passwordHasher.ValidatePassword(hash, request.Password))
        {
            return AuthPasswordResult.Failure();
        }

        return null;
    }
}