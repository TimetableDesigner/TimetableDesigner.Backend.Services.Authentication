using TimetableDesigner.Backend.Services.Authentication.Database.Model;

namespace TimetableDesigner.Backend.Services.Authentication.Core.Helpers;

public interface IAccessTokenGenerator
{
    string GenerateAccessToken(Account account);
    RefreshToken GenerateRefreshToken(bool isExtendable);
    bool ValidateExpiredAccessToken(string accessToken);
}