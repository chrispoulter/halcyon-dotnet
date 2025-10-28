namespace Halcyon.Api.Features.Users.UpdateUser;

public record UpdateUserRequest(
    string EmailAddress,
    string FirstName,
    string LastName,
    DateOnly DateOfBirth,
    List<string>? Roles,
    uint? Version
);

public record UpdateUserResponse(Guid Id);
