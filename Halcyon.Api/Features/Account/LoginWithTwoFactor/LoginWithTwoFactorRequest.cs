using FluentValidation;

namespace Halcyon.Api.Features.Account.LoginWithTwoFactor;

public record LoginWithTwoFactorRequest(string EmailAddress, string Password, string Code);

public class LoginWithTwoFactorRequestValidator : AbstractValidator<LoginWithTwoFactorRequest>
{
    public LoginWithTwoFactorRequestValidator()
    {
        RuleFor(x => x.EmailAddress)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(255)
            .WithName("Email Address");

        RuleFor(x => x.Password).NotEmpty().WithName("Password");
        RuleFor(x => x.Code).NotEmpty().Length(6).Matches("^[0-9]{6}$").WithName("Authenticator Code");
    }
}
