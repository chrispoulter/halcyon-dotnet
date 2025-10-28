namespace Halcyon.Api.Features.Account.Register;

public class RegisterRequest
{
    public string EmailAddress { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }
}
