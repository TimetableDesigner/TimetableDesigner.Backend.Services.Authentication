namespace TimetableDesigner.Backend.Services.Authentication.Application.Helpers;

public record PasswordHashData(
    byte[] Hash,
    string Salt
);