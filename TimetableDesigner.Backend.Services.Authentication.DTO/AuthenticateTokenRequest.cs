namespace TimetableDesigner.Backend.Services.Authentication.DTO;

public class AuthenticateTokenRequest
{
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
}