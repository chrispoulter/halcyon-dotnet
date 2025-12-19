namespace Halcyon.Api.Common.Authentication;

public interface ISecretHasher
{
    string GenerateHash(string str);

    bool VerifyHash(string str, string hash);
}
