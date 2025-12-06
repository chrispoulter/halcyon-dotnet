namespace Halcyon.Api.Features.Users.GetUser;

public record GetUserResponse(
    Guid Id,
    string EmailAddress,
    string FirstName,
    string LastName,
    DateOnly DateOfBirth,
    bool IsTwoFactorEnabled,
    bool IsLockedOut,
    IEnumerable<string>? Roles
);
