using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;

namespace TimetableDesigner.Backend.Services.Authentication.Core.Helpers;

public class PasswordHasher : IPasswordHasher
{
    private const string RandomPasswordCharacters = "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm1234567890!@#$%^&*()-_=+[{]};:'\"\\|,<.>/?";

    public PasswordHashData CreateHash(string password)
    {
        string salt = RandomNumberGenerator.GetString(RandomPasswordCharacters, 20);
        byte[] hash = ComputeHash(password, salt);
        PasswordHashData data = new PasswordHashData(hash, salt);
        return data;
    }

    public bool ValidatePassword(PasswordHashData hash, string password)
    {
        byte[] actualHash = hash.Hash;
        byte[] checkedHash = ComputeHash(password, hash.Salt);
        bool checkResult = checkedHash.SequenceEqual(actualHash);
        return checkResult;
    }

    protected byte[] ComputeHash(string password, string salt)
    {
        Argon2id hashFunction = new Argon2id(Encoding.UTF8.GetBytes(password))
        {
            Salt = Encoding.UTF8.GetBytes(salt),
            DegreeOfParallelism = 8,
            MemorySize = 65536,
            Iterations = 4
        };
        byte[] hash = hashFunction.GetBytes(32);
        return hash;
    }
}

public record PasswordHashData(
    byte[] Hash,
    string Salt
);