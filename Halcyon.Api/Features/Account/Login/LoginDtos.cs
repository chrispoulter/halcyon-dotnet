namespace Halcyon.Api.Features.Account.Login;

public record LoginRequest(string EmailAddress, string Password);

public record LoginResponse(string AccessToken);
