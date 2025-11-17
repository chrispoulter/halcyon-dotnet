namespace Halcyon.Api.Features.Profile.TwoFactor.RegenerateRecovery;

public record RegenerateRecoveryCodesResponse(Guid UserId, IEnumerable<string> RecoveryCodes);
