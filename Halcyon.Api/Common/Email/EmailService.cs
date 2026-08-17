using System.Diagnostics;
using System.Reflection;
using FluentEmail.Core;

namespace Halcyon.Api.Common.Email;

public class EmailService(
    IFluentEmail fluentEmail,
    EmailMetrics emailMetrics,
    ILogger<EmailService> logger
) : IEmailService
{
    private static readonly ActivitySource ActivitySource = new(EmailMetrics.MeterName);

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

        try
        {
            var sendResponse = await fluentEmail
                .To(toAddress)
                .Subject(subject)
                .UsingTemplateFromEmbedded(template, model, Assembly.GetExecutingAssembly())
                .SendAsync(cancellationToken);

            var durationSeconds = Stopwatch.GetElapsedTime(startTimestamp).TotalSeconds;

            emailMetrics.RecordEmailSendDuration(
                durationSeconds,
                template,
                sendResponse.Successful
            );

            if (!sendResponse.Successful)
            {
                var errorMessage = string.Join("; ", sendResponse.ErrorMessages);

                activity?.SetStatus(ActivityStatusCode.Error, errorMessage);

                logger.LogWarning(
                    "Email send reported failure for template {Template} to {ToAddress}: {ErrorMessage}",
                    template,
                    toAddress,
                    errorMessage
                );
            }

            return sendResponse.Successful;
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            var durationSeconds = Stopwatch.GetElapsedTime(startTimestamp).TotalSeconds;

            emailMetrics.RecordEmailSendDuration(durationSeconds, template, successful: false);

            activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
            activity?.AddException(ex);

            logger.LogError(
                ex,
                "Exception occurred while sending email for template {Template} to {ToAddress}",
                template,
                toAddress
            );

            return false;
        }
    }
}
