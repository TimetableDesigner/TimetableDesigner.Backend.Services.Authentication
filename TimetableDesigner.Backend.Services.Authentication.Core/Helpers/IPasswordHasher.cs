namespace TimetableDesigner.Backend.Services.Authentication.Core.Helpers;

public interface IPasswordHasher
{
    PasswordHashData CreateHash(string password);
    bool ValidatePassword(PasswordHashData hash, string password);
}