namespace TimetableDesigner.Backend.Services.Authentication.Core.Commands.AuthPassword;

public record AuthPasswordResult
{
    public bool IsSuccess { get; }
    public string? AccessToken { get; }
    public string? RefreshToken { get; }

    private AuthPasswordResult(bool isSuccess, string? accessToken, string? refreshToken)
    {
        IsSuccess = isSuccess;
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }

    public static AuthPasswordResult Success(string accessToken, string refreshToken) =>
        new AuthPasswordResult(true, accessToken, refreshToken);
    
    public static AuthPasswordResult Failure() =>
        new AuthPasswordResult(false, null, null);
}