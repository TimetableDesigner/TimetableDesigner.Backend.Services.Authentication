using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TimetableDesigner.Backend.Services.Authentication.Database;
using TimetableDesigner.Backend.Services.Authentication.Database.Model;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace TimetableDesigner.Backend.Services.Authentication.Core.Helpers;

public class AccessTokenGenerator : IAccessTokenGenerator
{
    private readonly IConfiguration _configuration;
    private readonly DatabaseContext _databaseContext;
    
    public AccessTokenGenerator(IConfiguration configuration, DatabaseContext databaseContext)
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

    public RefreshToken GenerateRefreshToken(bool isExtendable)
    {
        string lifetimeSection = isExtendable ? "Extended" : "Normal";
        int lifetime = _configuration.GetSection("Tokens")
                                     .GetSection("RefreshToken")
                                     .GetSection("Lifetime")
                                     .GetValue<int>(lifetimeSection);
        
        Guid guid = Guid.NewGuid();
        DateTimeOffset expirationDate = DateTimeOffset.UtcNow.AddMinutes(lifetime);

        return new RefreshToken
        {
            Token = guid,
            ExpirationDate = expirationDate,
            IsExtendable = isExtendable,
        };
    }

    public bool ValidateExpiredAccessToken(string accessToken)
    {
        IConfigurationSection accessTokenSettings = _configuration.GetSection("Tokens")
                                                                  .GetSection("AccessToken");
        
        string stringKey = accessTokenSettings.GetValue<string>("Key")!;
        byte[] encodedKey = Encoding.UTF8.GetBytes(stringKey);
        SymmetricSecurityKey key = new SymmetricSecurityKey(encodedKey);

        string algorithm = accessTokenSettings.GetValue<string>("Algorithm")!;
        
        TokenValidationParameters tokenValidation = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = false,
            ValidIssuer = accessTokenSettings.GetValue<string>("Issuer"),
            ValidAudience = accessTokenSettings.GetValue<string>("Audience"),
            IssuerSigningKey = key,
            ClockSkew = TimeSpan.FromMinutes(1),
        };
        
        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        tokenHandler.ValidateToken(accessToken, tokenValidation, out SecurityToken validatedToken);
        JwtSecurityToken? jwtSecurityToken = validatedToken as JwtSecurityToken;
        
        return jwtSecurityToken is not null && jwtSecurityToken.Header.Alg.Equals(algorithm, StringComparison.InvariantCultureIgnoreCase);
    }
}