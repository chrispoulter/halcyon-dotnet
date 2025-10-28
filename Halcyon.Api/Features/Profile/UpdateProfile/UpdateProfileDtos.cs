namespace Halcyon.Api.Features.Profile.UpdateProfile;

public class UpdateProfileRequest : UpdateRequest
{
    public string EmailAddress { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }
}
