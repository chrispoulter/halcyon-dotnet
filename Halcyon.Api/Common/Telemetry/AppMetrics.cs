using System.Diagnostics.Metrics;

namespace Halcyon.Api.Common.Telemetry;

public sealed class AppMetrics
{
    public const string MeterName = "Halcyon.Api";

    private readonly Counter<long> _loginAttempts;
    private readonly Counter<long> _accountLockoutChanges;
    private readonly Counter<long> _userRegistrations;

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

        _userRegistrations = meter.CreateCounter<long>(
            name: "account.registrations",
            unit: "{registration}",
            description: "Number of new user accounts created, tagged by source."
        );
    }

    public void RecordLoginAttempt(string result) =>
        _loginAttempts.Add(1, new KeyValuePair<string, object?>("result", result));

    public void RecordAccountLockoutChange(string action) =>
        _accountLockoutChanges.Add(1, new KeyValuePair<string, object?>("action", action));

    public void RecordUserRegistration(string source) =>
        _userRegistrations.Add(1, new KeyValuePair<string, object?>("source", source));
}
