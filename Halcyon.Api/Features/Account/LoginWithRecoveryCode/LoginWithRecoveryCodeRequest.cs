using FluentValidation;

namespace Halcyon.Api.Features.Account.LoginWithRecoveryCode;

public record LoginWithRecoveryCodeRequest(
    string EmailAddress,
    string Password,
    string RecoveryCode
);

public class LoginWithRecoveryCodeRequestValidator : AbstractValidator<LoginWithRecoveryCodeRequest>
{
    public LoginWithRecoveryCodeRequestValidator()
    {
        RuleFor(x => x.EmailAddress)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(255)
            .WithName("Email Address");

        RuleFor(x => x.Password).NotEmpty().WithName("Password");

        RuleFor(x => x.RecoveryCode).NotEmpty().Matches("^[A-F0-9]{10}$").WithName("Recovery Code");
    }
}
