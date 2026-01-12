namespace TimetableDesigner.Backend.Services.Authentication.DTO.API;

public class RegisterRequest
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string PasswordConfirmation { get; set; } = null!;
}