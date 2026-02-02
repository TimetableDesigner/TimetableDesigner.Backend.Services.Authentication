using TimetableDesigner.Backend.Services.Authentication.Database.Model;

namespace TimetableDesigner.Backend.Services.Authentication.Core.Helpers;

public interface ITokenGenerator
{
    string GenerateAccessToken(Account account);
    Task<string> GenerateRefreshTokenAsync(Account account, bool isExtendable);
    Task<string> ExtendRefreshTokenAsync();
}