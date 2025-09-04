using System;
using System.Security.Cryptography;
using Tutor.Application.Interfaces;

namespace Tutor.Application.Services;

public class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16;
    private const int HashSize = 20;
    private const int Iterations = 10000;

    public string GenerateSalt()
    {
        byte[] salt = new byte[SaltSize];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }
        return Convert.ToBase64String(salt);
    }

    public string HashPassword(string password, string salt)
    {
        byte[] saltBytes = Convert.FromBase64String(salt);
        
        using (var deriveBytes = new Rfc2898DeriveBytes(password, saltBytes, Iterations, HashAlgorithmName.SHA256))
        {
            byte[] hash = deriveBytes.GetBytes(HashSize);
            byte[] hashBytes = new byte[SaltSize + HashSize];
            
            Array.Copy(saltBytes, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);
            
            return Convert.ToBase64String(hashBytes);
        }
    }

    public bool VerifyPassword(string password, string hashedPassword, string salt)
    {
        byte[] hashBytes = Convert.FromBase64String(hashedPassword);
        byte[] saltBytes = Convert.FromBase64String(salt);
        
        byte[] storedHash = new byte[HashSize];
        Array.Copy(hashBytes, SaltSize, storedHash, 0, HashSize);
        
        using (var deriveBytes = new Rfc2898DeriveBytes(password, saltBytes, Iterations, HashAlgorithmName.SHA256))
        {
            byte[] computedHash = deriveBytes.GetBytes(HashSize);
            return CryptographicOperations.FixedTimeEquals(computedHash, storedHash);
        }
    }
}