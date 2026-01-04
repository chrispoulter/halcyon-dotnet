namespace Halcyon.Api.Common.Authentication;

public interface IEncryptionService
{
    public string EncryptSecret(string text);

    public string DecryptSecret(string cipherText);
}
