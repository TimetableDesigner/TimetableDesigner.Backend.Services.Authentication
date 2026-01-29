using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TimetableDesigner.Backend.Services.Authentication.Database;
using TimetableDesigner.Backend.Services.Authentication.Database.Model;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace TimetableDesigner.Backend.Services.Authentication.Core.Helpers;

public class TokenGenerator : ITokenGenerator
{
    private readonly IConfiguration _configuration;
    private readonly DatabaseContext _databaseContext;
    
    public TokenGenerator(IConfiguration configuration, DatabaseContext databaseContext)
    {
        _configuration = configuration;
        _databaseContext = databaseContext;
    }
    
    public string GenerateAccessToken(Account account)
    {
        IConfigurationSection accessTokenSettings = _configuration.GetSection("Tokens")
                                                                  .GetSection("AccessToken");

        int lifetime = accessTokenSettings.GetSection("Lifetime")
                                          .GetValue<int>("Normal");
        DateTimeOffset expirationDate = DateTimeOffset.UtcNow.AddMinutes(lifetime);
        
        string stringKey = accessTokenSettings.GetValue<string>("Key")!;
        byte[] encodedKey = Encoding.UTF8.GetBytes(stringKey);
        SymmetricSecurityKey key = new SymmetricSecurityKey(encodedKey);

        string algorithm = accessTokenSettings.GetValue<string>("Algorithm")!;

        SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, account.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, expirationDate.UtcTicks.ToString())
            ]),
            Issuer = accessTokenSettings.GetValue<string>("Issuer"),
            Audience = accessTokenSettings.GetValue<string>("Audience"),
            SigningCredentials = new SigningCredentials(key, algorithm),
            Expires = expirationDate.UtcDateTime,
        };

        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
        handler.InboundClaimTypeMap.Clear();
        SecurityToken token = handler.CreateToken(descriptor);

        return handler.WriteToken(token);
    }

    public async Task<string> GenerateRefreshTokenAsync(Account account)
    {
        return null;
    }
    
    public async Task<string> ExtendRefreshTokenAsync()
    {
        return null;
    }
}