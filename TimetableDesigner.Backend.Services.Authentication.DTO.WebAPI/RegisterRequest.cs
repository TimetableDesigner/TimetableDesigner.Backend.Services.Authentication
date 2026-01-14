namespace TimetableDesigner.Backend.Services.Authentication.DTO.WebAPI;

public record RegisterRequest(
    string Email, 
    string Password, 
    string PasswordConfirmation
);