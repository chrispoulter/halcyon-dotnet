namespace Halcyon.Api.Common.Email;

public interface IEmailService
{
    Task<bool> SendTemplateEmailAsync(
        string toAddress,
        string subject,
        string template,
        object model,
        CancellationToken cancellationToken = default
    );
}
