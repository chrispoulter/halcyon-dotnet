using FluentValidation;

namespace Halcyon.Api.Features.Profile.ChangePassword;

public class ChangePasswordRequest : UpdateRequest
{
    public string CurrentPassword { get; set; } = null!;

    public string NewPassword { get; set; } = null!;
}
