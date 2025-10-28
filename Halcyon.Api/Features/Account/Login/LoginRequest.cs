using FluentValidation;

namespace Halcyon.Api.Features.Account.Login;

public record LoginRequest(string EmailAddress, string Password);

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.EmailAddress)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(255)
            .WithName("Email Address");

        RuleFor(x => x.Password).NotEmpty().WithName("Password");
    }
}
