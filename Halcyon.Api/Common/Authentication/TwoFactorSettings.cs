namespace Halcyon.Api.Common.Authentication;

public class TwoFactorSettings
{
    public static string SectionName { get; } = "TwoFactor";

    public string Issuer { get; set; } = null!;
}
