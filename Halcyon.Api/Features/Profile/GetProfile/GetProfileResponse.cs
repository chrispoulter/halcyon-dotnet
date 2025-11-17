namespace Halcyon.Api.Features.Profile.GetProfile;

public record GetProfileResponse(
    Guid Id,
    string EmailAddress,
    string FirstName,
    string LastName,
    DateOnly DateOfBirth,
    bool IsTwoFactorEnabled,
    uint Version
);
