namespace Halcyon.Api.Common.Telemetry;

public static class TelemetryExtensions
{
    public static IHostApplicationBuilder AddTelemetryServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddSingleton<AppMetrics>();

        return builder;
    }
}
