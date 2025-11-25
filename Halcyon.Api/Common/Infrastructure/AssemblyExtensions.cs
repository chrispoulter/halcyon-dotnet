using System.Reflection;
using System.Text.RegularExpressions;

namespace Halcyon.Api.Common.Infrastructure;

public static partial class AssemblyExtensions
{
    public static string GetFullSemVer(this Assembly assembly)
    {
        var informationalVersion =
            assembly
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                ?.InformationalVersion ?? string.Empty;

        var match = FullSemVerPattern().Match(informationalVersion);

        return match.Success ? match.Groups[1].Value : informationalVersion;
    }

    [GeneratedRegex(@"^(\d+\.\d+\.\d+(?:-[0-9A-Za-z.-]+)?(?:\+[0-9A-Za-z]+)?)")]
    private static partial Regex FullSemVerPattern();
}
