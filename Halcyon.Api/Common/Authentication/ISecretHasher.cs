namespace Halcyon.Api.Common.Authentication;

public interface ISecretHasher
{
    string GenerateHash(string value);

    bool VerifyHash(string value, string hash);
}
