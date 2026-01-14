namespace TimetableDesigner.Backend.Services.Authentication.DTO.WebAPI;

public class AuthenticateTokenRequest
{
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
}