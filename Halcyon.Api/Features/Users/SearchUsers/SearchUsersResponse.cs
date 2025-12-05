namespace Halcyon.Api.Features.Users.SearchUsers;

public record SearchUsersResponse(
    IEnumerable<SearchUserResponse>? Items,
    bool HasNextPage,
    bool HasPreviousPage
);

public record SearchUserResponse(
    Guid Id,
    string EmailAddress,
    string FirstName,
    string LastName,
    bool IsLockedOut,
    IEnumerable<string>? Roles
);
