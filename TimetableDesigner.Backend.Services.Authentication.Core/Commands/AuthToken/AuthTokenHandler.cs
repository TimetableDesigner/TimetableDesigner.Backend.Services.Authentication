using MediatR;
using Microsoft.EntityFrameworkCore;
using TimetableDesigner.Backend.Services.Authentication.Core.Helpers;
using TimetableDesigner.Backend.Services.Authentication.Database;
using TimetableDesigner.Backend.Services.Authentication.Database.Model;

namespace TimetableDesigner.Backend.Services.Authentication.Core.Commands.AuthToken;

public class AuthTokenHandler : IRequestHandler<AuthTokenCommand, AuthTokenResult>
{
    private readonly DatabaseContext _databaseContext;
    private readonly IAccessTokenGenerator _accessTokenGenerator;

    public AuthTokenHandler(DatabaseContext databaseContext, IAccessTokenGenerator accessTokenGenerator)
    {
        _databaseContext = databaseContext;
        _accessTokenGenerator = accessTokenGenerator;
    }
    
    public async Task<AuthTokenResult> Handle(AuthTokenCommand request, CancellationToken cancellationToken)
    {
        RefreshToken? token = await _databaseContext.RefreshTokens
                                                    .Include(x => x.Account)
                                                    .FirstOrDefaultAsync(x => x.Token == Guid.Parse(request.RefreshToken), cancellationToken);
        if (token is null || token.ExpirationDate < DateTimeOffset.UtcNow || !_accessTokenGenerator.ValidateExpiredAccessToken(request.AccessToken))
        {
            return AuthTokenResult.Failure();
        }
        
        string accessToken = _accessTokenGenerator.GenerateAccessToken(token.Account);
        if (token.IsExtendable)
        {
            
        }
        

        return AuthTokenResult.Success(refreshToken, accessToken);
    }
}