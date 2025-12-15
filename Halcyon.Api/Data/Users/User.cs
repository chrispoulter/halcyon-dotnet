using Halcyon.Api.Common.Authentication;
using NpgsqlTypes;

namespace Halcyon.Api.Data.Users;

public class User : IJwtUser
{
    public Guid Id { get; set; }

    public string EmailAddress { get; set; } = null!;

    public string? Password { get; set; }

    public string? PasswordResetToken { get; set; }

    public bool IsTwoFactorEnabled { get; set; }

    public string? TwoFactorSecret { get; set; }

    public IEnumerable<string>? TwoFactorRecoveryCodes { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }

    public bool IsLockedOut { get; set; }

    public IEnumerable<string>? Roles { get; set; }

    public NpgsqlTsVector SearchVector { get; } = null!;
}
