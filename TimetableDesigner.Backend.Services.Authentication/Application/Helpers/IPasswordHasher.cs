namespace TimetableDesigner.Backend.Services.Authentication.Application.Helpers;

public interface IPasswordHasher
{
    PasswordHashData CreateHash(string password);
    byte[] ComputeHash(string password, string salt);
}