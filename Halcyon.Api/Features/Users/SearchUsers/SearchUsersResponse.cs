namespace Halcyon.Api.Features.Users.SearchUsers;

public record SearchUsersResponse(
    List<SearchUserResponse>? Items,
    bool HasNextPage,
    bool HasPreviousPage
);

public record SearchUserResponse(
    Guid Id,
    string EmailAddress,
    string FirstName,
    string LastName,
    bool IsTwoFactorEnabled,
    bool IsLockedOut,
    List<string>? Roles
);
