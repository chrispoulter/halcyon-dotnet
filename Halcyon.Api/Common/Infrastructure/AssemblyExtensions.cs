using System.Reflection;
using System.Text.RegularExpressions;

namespace Halcyon.Api.Common.Infrastructure;

public static partial class AssemblyExtensions
{
    public static string? GetSemVerShortSha(this Assembly assembly)
    {
        var informationalVersion = assembly
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
            ?.InformationalVersion;

        if (informationalVersion is null)
        {
            return null;
        }

        var match = SemVerShortShaPattern().Match(informationalVersion);

        return match.Success
            ? $"{match.Groups["semver"].Value}-{match.Groups["commit"].Value}"
            : null;
    }

    [GeneratedRegex(
        @"^(?<semver>\d+\.\d+\.\d+(?:-[0-9A-Za-z.-]+)?)\+.*?(?:Sha\.)?(?<commit>[0-9A-Fa-f]{7})[0-9A-Fa-f]{33}(?:\.[0-9A-Fa-f]{40})?"
    )]
    private static partial Regex SemVerShortShaPattern();
}
