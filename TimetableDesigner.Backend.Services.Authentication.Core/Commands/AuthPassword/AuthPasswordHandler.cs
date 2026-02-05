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
    private readonly ITokenHelper _tokenHelper;

    public AuthPasswordHandler(DatabaseContext databaseContext, IPasswordHasher passwordHasher, ITokenHelper tokenHelper)
    {
        _databaseContext = databaseContext;
        _passwordHasher = passwordHasher;
        _tokenHelper = tokenHelper;
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
        
        string accessToken = _tokenHelper.GenerateAccessToken(account.Id);
        
        RefreshToken refreshToken = new RefreshToken
        {
            Token = Guid.NewGuid(),
            IsExtendable = request.RememberMe,
            AccountId = account.Id,
            ExpirationDate = _tokenHelper.CalculateRefreshTokenExpirationDate(request.RememberMe),
        };
        
        await _databaseContext.RefreshTokens.AddAsync(refreshToken, cancellationToken);
        await _databaseContext.SaveChangesAsync(cancellationToken);

        return AuthPasswordResult.Success(accessToken, refreshToken.Token.ToString());
    }
}