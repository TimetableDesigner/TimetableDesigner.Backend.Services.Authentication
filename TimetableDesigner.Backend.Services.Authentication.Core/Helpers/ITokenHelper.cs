using TimetableDesigner.Backend.Services.Authentication.Database.Model;

namespace TimetableDesigner.Backend.Services.Authentication.Core.Helpers;

public interface ITokenHelper
{
    string GenerateAccessToken(long accountId);
    bool ValidateExpiredAccessToken(string accessToken);
    DateTimeOffset CalculateRefreshTokenExpirationDate(bool isExtendable = true);
}