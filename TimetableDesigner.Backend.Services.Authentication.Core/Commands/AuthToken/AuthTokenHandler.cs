using MediatR;
using Microsoft.EntityFrameworkCore;
using TimetableDesigner.Backend.Services.Authentication.Core.Helpers;
using TimetableDesigner.Backend.Services.Authentication.Database;
using TimetableDesigner.Backend.Services.Authentication.Database.Model;

namespace TimetableDesigner.Backend.Services.Authentication.Core.Commands.AuthToken;

public class AuthTokenHandler : IRequestHandler<AuthTokenCommand, AuthTokenResult>
{
    private readonly DatabaseContext _databaseContext;
    private readonly ITokenHelper _tokenHelper;

    public AuthTokenHandler(DatabaseContext databaseContext, ITokenHelper tokenHelper)
    {
        _databaseContext = databaseContext;
        _tokenHelper = tokenHelper;
    }
    
    public async Task<AuthTokenResult> Handle(AuthTokenCommand request, CancellationToken cancellationToken)
    {
        RefreshToken? refreshToken = await _databaseContext.RefreshTokens
                                                    .Include(x => x.Account)
                                                    .FirstOrDefaultAsync(x => x.Token == Guid.Parse(request.RefreshToken), cancellationToken);
        if (refreshToken is null || refreshToken.ExpirationDate < DateTimeOffset.UtcNow || !_tokenHelper.ValidateExpiredAccessToken(request.AccessToken))
        {
            return AuthTokenResult.Failure();
        }
        
        string accessToken = _tokenHelper.GenerateAccessToken(refreshToken.Account.Id);
        
        if (refreshToken.IsExtendable)
        {
            refreshToken.ExpirationDate = _tokenHelper.CalculateRefreshTokenExpirationDate();
            await _databaseContext.SaveChangesAsync(cancellationToken);
        }

        return AuthTokenResult.Success(accessToken, refreshToken.Token.ToString());
    }
}