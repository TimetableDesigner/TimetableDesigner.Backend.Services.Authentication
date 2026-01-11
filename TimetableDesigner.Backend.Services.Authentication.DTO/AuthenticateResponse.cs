namespace TimetableDesigner.Backend.Services.Authentication.DTO;

public class AuthenticateResponse
{
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
}