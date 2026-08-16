using System.Diagnostics.Metrics;
using Halcyon.Api.Common.Telemetry;

namespace Halcyon.Api.Common.Email;

public class EmailMetrics
{
    public const string MeterName = "Halcyon.Api.Common.Email";

    private readonly Histogram<double> _emailSendDuration;

    public EmailMetrics(IMeterFactory meterFactory)
    {
        var meter = meterFactory.Create(AppMetrics.MeterName);

        _emailSendDuration = meter.CreateHistogram<double>(
            name: "email.send.duration",
            unit: "s",
            description: "Duration of the email send operation, in seconds, tagged by type and result."
        );
    }

    public void RecordEmailSendDuration(double durationSeconds, string template, bool successful) =>
        _emailSendDuration.Record(
            durationSeconds,
            new KeyValuePair<string, object?>("template", template),
            new KeyValuePair<string, object?>("result", successful ? "success" : "failure")
        );
}
