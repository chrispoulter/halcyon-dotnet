namespace Halcyon.Api.Features.Users.CreateUser;

public class CreateUserRequest
{
    public string EmailAddress { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }

    public List<string> Roles { get; set; } = null!;
}
