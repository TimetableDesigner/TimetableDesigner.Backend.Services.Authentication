namespace TimetableDesigner.Backend.Services.Authentication.DTO.WebAPI;

public record AuthPasswordRequest
(
    string Email, 
    string Password, 
    bool RememberMe
);