namespace TimetableDesigner.Backend.Services.Authentication.DTO.API;

public class AuthenticatePasswordRequest
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public bool RememberMe { get; set; }
}