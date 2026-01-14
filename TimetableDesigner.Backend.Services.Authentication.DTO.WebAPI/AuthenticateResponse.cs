namespace TimetableDesigner.Backend.Services.Authentication.DTO.WebAPI;

public class AuthenticateResponse
{
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
}