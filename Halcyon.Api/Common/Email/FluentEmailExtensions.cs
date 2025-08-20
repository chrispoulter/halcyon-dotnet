using FluentEmail.MailKitSmtp;

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

        var socketOptions = emailSettings.SmtpSsl
            ? MailKit.Security.SecureSocketOptions.StartTls
            : MailKit.Security.SecureSocketOptions.None;

        var requiresAuthentication =
            !string.IsNullOrEmpty(emailSettings.SmtpUserName)
            && !string.IsNullOrEmpty(emailSettings.SmtpPassword);

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
                    SocketOptions = socketOptions,
                    RequiresAuthentication = requiresAuthentication,
                    User = emailSettings.SmtpUserName,
                    Password = emailSettings.SmtpPassword,
                }
            );

        return builder;
    }
}
