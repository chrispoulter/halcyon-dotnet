using System.Diagnostics.Metrics;

namespace Halcyon.Api.Common.Telemetry;

public sealed class AppMetrics
{
    public const string MeterName = "Halcyon.Api";

    private readonly Counter<long> _loginAttempts;
    private readonly Counter<long> _accountLockoutChanges;
    private readonly Counter<long> _emailsSent;
    private readonly Counter<long> _userRegistrations;
    private readonly Histogram<double> _userSearchDuration;

    public AppMetrics(IMeterFactory meterFactory)
    {
        var meter = meterFactory.Create(MeterName);

        _loginAttempts = meter.CreateCounter<long>(
            name: "account.login.attempts",
            unit: "{attempt}",
            description: "Number of login attempts, tagged by result."
        );

        _accountLockoutChanges = meter.CreateCounter<long>(
            name: "account.lockout.changes",
            unit: "{change}",
            description: "Number of times an account was locked or unlocked, tagged by action."
        );

        _emailsSent = meter.CreateCounter<long>(
            name: "email.sent",
            unit: "{email}",
            description: "Number of emails sent, tagged by type and result."
        );

        _userRegistrations = meter.CreateCounter<long>(
            name: "account.registrations",
            unit: "{registration}",
            description: "Number of new user accounts created, tagged by source."
        );

        _userSearchDuration = meter.CreateHistogram<double>(
            name: "users.search.duration",
            unit: "s",
            description: "Duration of the user search request, in seconds, tagged by whether a search term was supplied."
        );
    }

    public void RecordLoginAttempt(string result) =>
        _loginAttempts.Add(1, new KeyValuePair<string, object?>("result", result));

    public void RecordAccountLockoutChange(string action) =>
        _accountLockoutChanges.Add(1, new KeyValuePair<string, object?>("action", action));

    public void RecordEmailSent(string type, bool successful) =>
        _emailsSent.Add(
            1,
            new KeyValuePair<string, object?>("type", type),
            new KeyValuePair<string, object?>("result", successful ? "success" : "failure")
        );

    public void RecordUserRegistration(string source) =>
        _userRegistrations.Add(1, new KeyValuePair<string, object?>("source", source));

    public void RecordUserSearchDuration(double durationSeconds, bool hasSearchTerm) =>
        _userSearchDuration.Record(
            durationSeconds,
            new KeyValuePair<string, object?>("has_search_term", hasSearchTerm)
        );
}
