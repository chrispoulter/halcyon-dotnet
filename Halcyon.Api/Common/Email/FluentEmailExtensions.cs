using FluentEmail.MailKitSmtp;
using MailKit.Security;

namespace Halcyon.Api.Common.Email;

public static class FluentEmailExtensions
{
    public static IHostApplicationBuilder AddFluentEmail(
        this IHostApplicationBuilder builder,
        string connectionName
    )
    {
        var emailSettings =
            builder.Configuration.GetSection(EmailSettings.SectionName).Get<EmailSettings>()
            ?? throw new InvalidOperationException(
                "Email settings section is missing in configuration."
            );

        if (builder.Configuration.GetConnectionString(connectionName) is string connectionString)
        {
            emailSettings.ParseConnectionString(connectionString);
        }

        builder
            .Services.AddFluentEmail(emailSettings.NoReplyAddress)
            .AddLiquidRenderer(configure =>
            {
                configure.ConfigureTemplateContext = (context, _) =>
                {
                    context.SetValue("SiteUrl", emailSettings.SiteUrl);
                };
            })
            .AddMailKitSender(
                new SmtpClientOptions
                {
                    Server = emailSettings.SmtpServer,
                    Port = emailSettings.SmtpPort,
                    SocketOptions = emailSettings.SmtpSsl
                        ? SecureSocketOptions.StartTls
                        : SecureSocketOptions.None,
                    RequiresAuthentication =
                        !string.IsNullOrEmpty(emailSettings.SmtpUserName)
                        && !string.IsNullOrEmpty(emailSettings.SmtpPassword),
                    User = emailSettings.SmtpUserName,
                    Password = emailSettings.SmtpPassword,
                }
            );

        return builder;
    }
}
