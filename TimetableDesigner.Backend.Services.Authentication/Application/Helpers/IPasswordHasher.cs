namespace TimetableDesigner.Backend.Services.Authentication.Application.Helpers;

public interface IPasswordHasher
{
    PasswordHashData CreateHash(string password);
    bool ValidatePassword(PasswordHashData hash, string password);
}