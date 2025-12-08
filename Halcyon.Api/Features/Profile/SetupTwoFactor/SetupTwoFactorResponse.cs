namespace Halcyon.Api.Features.Profile.SetupTwoFactor;

public record SetupTwoFactorResponse(Guid Id, string OtpauthUri, string Secret);