using FluentValidation;

namespace Halcyon.Api.Features.Account.ForgotPassword;

public record ForgotPasswordRequest(string EmailAddress);

public class ForgotPasswordRequestValidator : AbstractValidator<ForgotPasswordRequest>
{
    public ForgotPasswordRequestValidator()
    {
        RuleFor(x => x.EmailAddress).NotEmpty().EmailAddress().WithName("Email Address");
    }
}
