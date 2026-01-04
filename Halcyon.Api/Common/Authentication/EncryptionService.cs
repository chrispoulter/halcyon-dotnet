using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;

namespace Halcyon.Api.Common.Authentication;

public class EncryptionService(IOptions<EncryptionSettings> options) : IEncryptionService
{
    private const int NonceSize = 12; 
    private const int TagSize = 16;

    private readonly byte[] Key = Convert.FromBase64String(options.Value.Key);

    public string EncryptSecret(string secret)
    {
        var plaintext = Encoding.UTF8.GetBytes(secret);
        var nonce = RandomNumberGenerator.GetBytes(NonceSize);
        var ciphertext = new byte[plaintext.Length];
        var tag = new byte[TagSize];

        using var aes = new AesGcm(Key, TagSize);
        aes.Encrypt(nonce, plaintext, ciphertext, tag);

        var combined = new byte[NonceSize + TagSize + ciphertext.Length];
        Buffer.BlockCopy(nonce, 0, combined, 0, NonceSize);
        Buffer.BlockCopy(tag, 0, combined, NonceSize, TagSize);
        Buffer.BlockCopy(ciphertext, 0, combined, NonceSize + TagSize, ciphertext.Length);

        return Convert.ToBase64String(combined);
    }

    public string DecryptSecret(string encryptedSecret)
    {
        var data = Convert.FromBase64String(encryptedSecret);
        var nonce = data[..NonceSize];
        var tag = data[NonceSize..(NonceSize + TagSize)];
        var ciphertext = data[(NonceSize + TagSize)..];
        var plaintext = new byte[ciphertext.Length];

        using var aes = new AesGcm(Key, TagSize);
        aes.Decrypt(nonce, ciphertext, tag, plaintext);

        return Encoding.UTF8.GetString(plaintext);
    }
}
