namespace TimetableDesigner.Backend.Services.Authentication.DTO.WebAPI;

public record AuthResponse
(
    string AccessToken, 
    string RefreshToken
);