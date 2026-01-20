namespace TimetableDesigner.Backend.Services.Authentication.Core.Helpers;

public record PasswordHashData(
    byte[] Hash,
    string Salt
);