using System.Reflection;
using System.Text.RegularExpressions;

namespace Halcyon.Api.Common.Infrastructure;

public static partial class AssemblyExtensions
{
    public static string GetSemverVersion(this Assembly assembly)
    {
        var informationalVersion =
            assembly
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                ?.InformationalVersion ?? string.Empty;

        var match = SemVerPattern().Match(informationalVersion);

        return match.Success ? match.Groups[1].Value : "1.0.0";
    }

    [GeneratedRegex(@"\b(\d+\.\d+\.\d+(?:-[0-9A-Za-z.-]+)?)")]
    private static partial Regex SemVerPattern();
}
