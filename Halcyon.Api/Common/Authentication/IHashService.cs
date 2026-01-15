namespace Halcyon.Api.Common.Authentication;

public interface IHashService
{
    string GenerateHash(string value);

    bool VerifyHash(string value, string hash);
}
