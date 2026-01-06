namespace Halcyon.Api.Common.Authentication;

public class EncryptionSettings
{
    public static string SectionName { get; } = "Encryption";

    public string Key { get; set; } = null!;
}
