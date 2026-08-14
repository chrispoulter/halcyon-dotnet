using System.Diagnostics;
using System.Reflection;
using FluentEmail.Core;
using Halcyon.Api.Common.Telemetry;

namespace Halcyon.Api.Common.Email;

public class EmailService(IServiceProvider serviceProvider, AppMetrics appMetrics) : IEmailService
{
    private static readonly ActivitySource ActivitySource = new(AppMetrics.MeterName);

    public async Task<bool> SendTemplateEmailAsync(
        string toAddress,
        string subject,
        string template,
        object model,
        CancellationToken cancellationToken = default
    )
    {
        using var activity = ActivitySource.StartActivity("email.send", ActivityKind.Client);
        activity?.SetTag("email.template", template);

        var startTimestamp = Stopwatch.GetTimestamp();

        var fluentEmail = serviceProvider.GetRequiredService<IFluentEmail>();

        var sendResponse = await fluentEmail
            .To(toAddress)
            .Subject(subject)
            .UsingTemplateFromEmbedded(template, model, Assembly.GetExecutingAssembly())
            .SendAsync(cancellationToken);

        var durationSeconds = Stopwatch.GetElapsedTime(startTimestamp).TotalSeconds;

        appMetrics.RecordEmailSent(template, sendResponse.Successful);
        appMetrics.RecordEmailSendDuration(durationSeconds, template, sendResponse.Successful);

        if (!sendResponse.Successful)
        {
            activity?.SetStatus(
                ActivityStatusCode.Error,
                string.Join("; ", sendResponse.ErrorMessages)
            );
        }

        return sendResponse.Successful;
    }
}
