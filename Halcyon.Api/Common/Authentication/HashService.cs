using System.Security.Cryptography;
using System.Text;

namespace Halcyon.Api.Common.Authentication;

public class HashService : IHashService
{
    private readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA256;

    private const int SaltSize = 16;

    private const int KeySize = 32;

    private const int Iterations = 10000;

    public string GenerateHash(string value)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);

        var key = Rfc2898DeriveBytes.Pbkdf2(
            password: Encoding.UTF8.GetBytes(value),
            salt: salt,
            iterations: Iterations,
            hashAlgorithm: Algorithm,
            outputLength: KeySize
        );

        var saltBase64 = Convert.ToBase64String(salt);
        var keyBase64 = Convert.ToBase64String(key);

        return $"{saltBase64}.{keyBase64}";
    }

    public bool VerifyHash(string value, string hash)
    {
        var parts = hash.Split('.', 2);
        if (parts.Length != 2)
        {
            return false;
        }

        var salt = Convert.FromBase64String(parts[0]);
        var key = Convert.FromBase64String(parts[1]);

        var keyToCheck = Rfc2898DeriveBytes.Pbkdf2(
            password: Encoding.UTF8.GetBytes(value),
            salt: salt,
            iterations: Iterations,
            hashAlgorithm: Algorithm,
            outputLength: KeySize
        );

        return CryptographicOperations.FixedTimeEquals(key, keyToCheck);
    }
}
