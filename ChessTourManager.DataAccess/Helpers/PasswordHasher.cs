using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ChessTourManager.DataAccess.Helpers;

/// Class to hash and verify passwords
public static class PasswordHasher
{
    private const           int               KeySize       = 64;
    private const           int               Iterations    = 350000;
    private static readonly HashAlgorithmName HashAlgorithm = HashAlgorithmName.SHA512;
    private static readonly byte[]            Salt          = { 255 };

    public static string HashPassword(string password)
    {
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password),
                                                Salt,
                                                Iterations,
                                                HashAlgorithm,
                                                KeySize);

        return Convert.ToHexString(hash);
    }

    public static bool VerifyPassword(string password, string hash)
    {
        byte[] hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, Salt, Iterations, HashAlgorithm, KeySize);
        return hashToCompare.SequenceEqual(Convert.FromHexString(hash));
    }
}
