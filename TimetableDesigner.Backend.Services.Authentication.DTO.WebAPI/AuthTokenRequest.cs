namespace TimetableDesigner.Backend.Services.Authentication.DTO.WebAPI;

public class AuthTokenRequest
{
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
}