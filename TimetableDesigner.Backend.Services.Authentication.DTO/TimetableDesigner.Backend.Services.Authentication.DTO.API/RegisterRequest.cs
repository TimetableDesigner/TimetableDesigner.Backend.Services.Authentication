namespace TimetableDesigner.Backend.Services.Authentication.DTO.API;

public record RegisterRequest(
    string Email, 
    string Password, 
    string PasswordConfirmation
);