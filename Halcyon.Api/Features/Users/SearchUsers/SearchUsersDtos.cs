namespace Halcyon.Api.Features.Users.SearchUsers;

public enum UserSort
{
    EMAIL_ADDRESS_ASC,
    EMAIL_ADDRESS_DESC,
    NAME_ASC,
    NAME_DESC,
}

public record SearchUsersRequest(string? Search, UserSort? Sort, int? Page, int? Size);

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
    bool IsLockedOut,
    List<string>? Roles
);
