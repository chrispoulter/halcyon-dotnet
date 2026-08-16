using System.Diagnostics.Metrics;
using Halcyon.Api.Common.Telemetry;

namespace Halcyon.Api.Common.Email;

public sealed class EmailMetrics
{
    private readonly Counter<long> _emailsSent;
    private readonly Histogram<double> _emailSendDuration;

    public EmailMetrics(IMeterFactory meterFactory)
    {
        var meter = meterFactory.Create(AppMetrics.MeterName);

        _emailsSent = meter.CreateCounter<long>(
            name: "email.sent",
            unit: "{email}",
            description: "Number of emails sent, tagged by type and result."
        );

        _emailSendDuration = meter.CreateHistogram<double>(
            name: "email.send.duration",
            unit: "s",
            description: "Duration of the email send operation, in seconds, tagged by type and result."
        );
    }

    public void RecordEmailSent(string template, bool successful) =>
        _emailsSent.Add(
            1,
            new KeyValuePair<string, object?>("template", template),
            new KeyValuePair<string, object?>("result", successful ? "success" : "failure")
        );

    public void RecordEmailSendDuration(double durationSeconds, string template, bool successful) =>
        _emailSendDuration.Record(
            durationSeconds,
            new KeyValuePair<string, object?>("template", template),
            new KeyValuePair<string, object?>("result", successful ? "success" : "failure")
        );
}
