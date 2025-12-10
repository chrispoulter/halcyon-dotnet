using FluentValidation;

namespace Halcyon.Api.Features.Account.ResetPassword;

public record ResetPasswordRequest(string Token, string EmailAddress, string NewPassword);

public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
{
    public ResetPasswordRequestValidator()
    {
        RuleFor(x => x.Token).NotEmpty().Matches("^[A-F0-9]{32}$");
        RuleFor(x => x.EmailAddress).NotEmpty().EmailAddress().WithName("Email Address");

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(50)
            .WithName("New Password");
    }
}
