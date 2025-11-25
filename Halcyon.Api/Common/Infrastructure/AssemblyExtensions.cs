using System.Reflection;
using System.Text.RegularExpressions;

namespace Halcyon.Api.Common.Infrastructure;

public static partial class AssemblyExtensions
{
    public static string GetTagVersion(this Assembly assembly)
    {
        var informationalVersion =
            assembly
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                ?.InformationalVersion ?? string.Empty;

        var match = DisplayVersionPattern().Match(informationalVersion);

        return match.Success
            ? $"{match.Groups[1].Value}-{match.Groups[2].Value}"
            : informationalVersion;
    }

    [GeneratedRegex(
        @"^(\d+\.\d+\.\d+(?:-[0-9A-Za-z.-]+)?)(?:\+[0-9A-Za-z]+)\.Branch\.[^.]+\.Sha\.([0-9A-Fa-f]{6})"
    )]
    private static partial Regex DisplayVersionPattern();
}
