using FluentValidation;

namespace Halcyon.Api.Features.Account.LoginTwoFactor;

public record LoginTwoFactorRequest(string EmailAddress, string Password, string? Code, string? RecoveryCode);

public class LoginTwoFactorRequestValidator : AbstractValidator<LoginTwoFactorRequest>
{
    public LoginTwoFactorRequestValidator()
    {
        RuleFor(x => x.EmailAddress)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(255)
            .WithName("Email Address");

        RuleFor(x => x.Password).NotEmpty().WithName("Password");

        RuleFor(x => x)
            .Must(x => !string.IsNullOrWhiteSpace(x.Code) || !string.IsNullOrWhiteSpace(x.RecoveryCode))
            .WithMessage("Provide either a 6-digit code or a recovery code.");

        When(x => !string.IsNullOrWhiteSpace(x.Code), () =>
        {
            RuleFor(x => x.Code!)
                .Length(6)
                .Matches("^[0-9]{6}$")
                .WithName("Two-factor code");
        });

        When(x => !string.IsNullOrWhiteSpace(x.RecoveryCode), () =>
        {
            RuleFor(x => x.RecoveryCode!)
                .MinimumLength(8)
                .MaximumLength(64)
                .WithName("Recovery code");
        });
    }
}
