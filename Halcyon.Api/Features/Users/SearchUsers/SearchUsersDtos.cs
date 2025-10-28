namespace Halcyon.Api.Features.Users.SearchUsers;

public enum UserSort
{
    EMAIL_ADDRESS_ASC,
    EMAIL_ADDRESS_DESC,
    NAME_ASC,
    NAME_DESC,
}

public class SearchUsersRequest
{
    public string? Search { get; set; }

    public UserSort? Sort { get; set; }

    public int? Page { get; set; }

    public int? Size { get; set; }
}

public class SearchUsersResponse
{
    public List<SearchUserResponse>? Items { get; set; }

    public bool HasNextPage { get; set; }

    public bool HasPreviousPage { get; set; }
}

public class SearchUserResponse
{
    public Guid Id { get; set; }

    public string EmailAddress { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public bool IsLockedOut { get; set; }

    public List<string>? Roles { get; set; }
}
