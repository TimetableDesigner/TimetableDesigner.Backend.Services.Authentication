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
    private readonly IAccessTokenGenerator _accessTokenGenerator;

    public AuthPasswordHandler(DatabaseContext databaseContext, IPasswordHasher passwordHasher, IAccessTokenGenerator accessTokenGenerator)
    {
        _databaseContext = databaseContext;
        _passwordHasher = passwordHasher;
        _accessTokenGenerator = accessTokenGenerator;
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
        
        string accessToken = _accessTokenGenerator.GenerateAccessToken(account);
        RefreshToken refreshToken = _accessTokenGenerator.GenerateRefreshToken(request.RememberMe);
        
        account.RefreshTokens.Add(refreshToken);
        await _databaseContext.SaveChangesAsync(cancellationToken);

        return AuthPasswordResult.Success(accessToken, refreshToken.Token.ToString());
    }
}