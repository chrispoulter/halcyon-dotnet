namespace Halcyon.Api.Features.Profile.VerifyTwoFactor;

public record VerifyTwoFactorResponse(Guid UserId, IEnumerable<string> RecoveryCodes);
