namespace Halcyon.Api.Features.Profile.SetupTwoFactor;

public record SetupTwoFactorResponse(Guid UserId, string Secret, string OtpAuthUri);
