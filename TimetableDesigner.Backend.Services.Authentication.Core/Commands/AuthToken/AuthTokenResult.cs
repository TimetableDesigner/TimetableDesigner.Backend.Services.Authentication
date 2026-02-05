namespace TimetableDesigner.Backend.Services.Authentication.Core.Commands.AuthToken;

public record AuthTokenResult
{
    public bool IsSuccess { get; }
    public string? AccessToken { get; }
    public string? RefreshToken { get; }

    private AuthTokenResult(bool isSuccess, string? accessToken, string? refreshToken)
    {
        IsSuccess = isSuccess;
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }

    public static AuthTokenResult Success(string accessToken, string refreshToken) =>
        new AuthTokenResult(true, accessToken, refreshToken);
    
    public static AuthTokenResult Failure() =>
        new AuthTokenResult(false, null, null);
}