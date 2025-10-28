namespace Halcyon.Api.Features.Profile.UpdateProfile;

public record UpdateProfileRequest(
    string EmailAddress,
    string FirstName,
    string LastName,
    DateOnly DateOfBirth,
    uint? Version
);

public record UpdateProfileResponse(Guid Id);
