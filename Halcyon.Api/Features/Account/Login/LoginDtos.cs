namespace Halcyon.Api.Features.Account.Login;

public class LoginRequest
{
    public string EmailAddress { get; set; } = null!;

    public string Password { get; set; } = null!;
}

public class LoginResponse
{
    public string AccessToken { get; set; } = null!;
}
