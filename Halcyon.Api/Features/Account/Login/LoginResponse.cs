namespace Halcyon.Api.Features.Account.Login;

public record LoginResponse(bool RequiresTwoFactor, string? AccessToken);
