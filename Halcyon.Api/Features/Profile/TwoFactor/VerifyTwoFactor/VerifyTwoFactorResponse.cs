namespace Halcyon.Api.Features.Profile.TwoFactor.VerifyTwoFactor;

public record VerifyTwoFactorResponse(Guid Id, bool Enabled, IEnumerable<string> RecoveryCodes);
