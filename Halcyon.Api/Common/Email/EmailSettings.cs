using System.Data.Common;

namespace Halcyon.Api.Common.Email;

public class EmailSettings
{
    public static string SectionName { get; } = "Email";

    public string SmtpServer { get; set; } = null!;

    public int SmtpPort { get; set; }

    public bool SmtpSsl { get; set; }

    public string SmtpUserName { get; set; } = null!;

    public string SmtpPassword { get; set; } = null!;

    public string NoReplyAddress { get; set; } = null!;

    public string SiteUrl { get; set; } = null!;

    internal void ParseConnectionString(string connectionString)
    {
        var connectionStringBuilder = new DbConnectionStringBuilder
        {
            ConnectionString = connectionString,
        };

        if (connectionStringBuilder.TryGetValue("Endpoint", out var endpoint))
        {
            if (Uri.TryCreate(endpoint.ToString(), UriKind.Absolute, out var uri))
            {
                SmtpServer = uri.Host;
                SmtpPort = uri.Port;
                SmtpSsl = uri.Scheme == "smtps";

                if (!string.IsNullOrEmpty(uri.UserInfo))
                {
                    var userInfoParts = uri.UserInfo.Split(':');
                    SmtpUserName = userInfoParts[0];
                    SmtpPassword = userInfoParts[1];
                }
            }
        }
    }
}
