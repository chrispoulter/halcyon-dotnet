namespace Halcyon.Api.Features.Profile.TwoFactor.SetupTwoFactor;

public record SetupTwoFactorResponse(string Id, string Secret, string OtpauthUri, string QrContent);
