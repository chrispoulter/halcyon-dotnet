using Halcyon.Api.Common.Authentication;
using NpgsqlTypes;

namespace Halcyon.Api.Data;

public class User : IJwtUser
{
    public Guid Id { get; set; }

    public string EmailAddress { get; set; } = null!;

    public string? Password { get; set; }

    public Guid? PasswordResetToken { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }

    public bool IsLockedOut { get; set; }

    public IEnumerable<string>? Roles { get; set; }

    public NpgsqlTsVector SearchVector { get; } = null!;
}
