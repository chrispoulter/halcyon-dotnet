using System.Diagnostics;
using System.Reflection;
using FluentEmail.Core;

namespace Halcyon.Api.Common.Email;

public class EmailService(IFluentEmail fluentEmail, EmailMetrics emailMetrics) : IEmailService
{
    private static readonly ActivitySource ActivitySource = new(EmailMetrics.MeterName);

    public async Task SendTemplateEmailAsync(
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
        var successful = false;

        try
        {
            var sendResponse = await fluentEmail
                .To(toAddress)
                .Subject(subject)
                .UsingTemplateFromEmbedded(template, model, Assembly.GetExecutingAssembly())
                .SendAsync(cancellationToken);

            if (!sendResponse.Successful)
            {
                var errorMessage = string.Join("; ", sendResponse.ErrorMessages);

                throw new Exception(
                    $"Failed to send email with template {template}: {errorMessage}"
                );
            }

            successful = true;
        }
        catch (Exception ex)
        {
            activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
            throw;
        }
        finally
        {
            var durationSeconds = Stopwatch.GetElapsedTime(startTimestamp).TotalSeconds;
            emailMetrics.RecordEmailSendDuration(durationSeconds, template, successful);
        }
    }
}
