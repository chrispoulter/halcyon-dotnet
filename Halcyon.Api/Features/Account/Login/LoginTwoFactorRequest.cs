using FluentValidation;

namespace Halcyon.Api.Features.Account.Login;

public record LoginTwoFactorRequest(string EmailAddress, string Password, string Code);

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
        RuleFor(x => x.Code).NotEmpty().Length(6).WithName("Two-factor code");
    }
}
