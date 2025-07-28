namespace Halcyon.Api.Features.System.GetVersion;

public class GetVersionResponse
{
    public string Version { get; set; } = null!;

    public DateTime UtcNow { get; set; }

    public DateOnly UtcNowDateOnly { get; set; }

    public DateTime Now { get; set; }

    public DateOnly NowDateOnly { get; set; }
}
