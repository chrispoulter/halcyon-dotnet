namespace Halcyon.Api.Features.Profile.GenerateRecoveryCodes;

public record GenerateRecoveryCodesResponse(Guid UserId, IEnumerable<string> RecoveryCodes);
