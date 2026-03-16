namespace Halcyon.Api.Features.Profile.VerifyTwoFactor;

public record VerifyTwoFactorResponse(Guid Id, IEnumerable<string> RecoveryCodes);
