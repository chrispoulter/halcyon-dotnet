namespace Halcyon.Api.Features.Profile.GenerateRecoveryCodes;

public record GenerateRecoveryCodesResponse(Guid Id, IEnumerable<string> RecoveryCodes);
