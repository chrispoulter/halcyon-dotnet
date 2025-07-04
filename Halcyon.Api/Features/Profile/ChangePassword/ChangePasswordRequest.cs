using FluentValidation;

namespace Halcyon.Api.Features.Profile.ChangePassword;

public class ChangePasswordRequest : UpdateRequest
{
    public string CurrentPassword { get; set; } = null!;

    public string NewPassword { get; set; } = null!;
}

public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordRequestValidator()
    {
        RuleFor(x => x.CurrentPassword).NotEmpty().WithName("Current Password");

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(50)
            .WithName("New Password");
    }
}
