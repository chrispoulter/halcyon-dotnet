namespace Halcyon.Api.Features.Profile.DeleteProfile;

public record DeleteProfileRequest(uint? Version);

public record DeleteProfileResponse(Guid Id);
