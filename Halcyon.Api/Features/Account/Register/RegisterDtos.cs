namespace Halcyon.Api.Features.Account.Register;

public record RegisterRequest(
    string EmailAddress,
    string Password,
    string FirstName,
    string LastName,
    DateOnly DateOfBirth
);

public record RegisterResponse(Guid Id);
