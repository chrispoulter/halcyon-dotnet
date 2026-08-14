namespace Halcyon.Api.Common.Email;

public interface IEmailService
{
    Task<bool> SendTemplateEmailAsync(
        string type,
        string toAddress,
        string subject,
        string templateResourceName,
        object model,
        CancellationToken cancellationToken = default
    );
}
