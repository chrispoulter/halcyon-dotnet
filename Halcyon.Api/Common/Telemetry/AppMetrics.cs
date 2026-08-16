using System.Diagnostics.Metrics;

namespace Halcyon.Api.Common.Telemetry;

public class AppMetrics
{
    public const string MeterName = "Halcyon.Api";

    private readonly Counter<long> _loginAttempts;

    public AppMetrics(IMeterFactory meterFactory)
    {
        var meter = meterFactory.Create(MeterName);

        _loginAttempts = meter.CreateCounter<long>(
            name: "account.login.attempts",
            unit: "{attempt}",
            description: "Number of login attempts, tagged by result."
        );
    }

    public void RecordLoginAttempt(string result) =>
        _loginAttempts.Add(1, new KeyValuePair<string, object?>("result", result));
}
