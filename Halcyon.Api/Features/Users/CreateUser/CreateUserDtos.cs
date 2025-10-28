namespace Halcyon.Api.Features.Users.CreateUser;

public record CreateUserRequest(
    string EmailAddress,
    string Password,
    string FirstName,
    string LastName,
    DateOnly DateOfBirth,
    List<string>? Roles
);

public record CreateUserResponse(Guid Id);
