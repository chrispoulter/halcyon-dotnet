using FluentValidation;

namespace Halcyon.Api.Features.Profile.VerifyTwoFactor;

public record VerifyTwoFactorRequest(string VerificationCode);

public class VerifyTwoFactorRequestValidator : AbstractValidator<VerifyTwoFactorRequest>
{
    public VerifyTwoFactorRequestValidator()
    {
        RuleFor(x => x.VerificationCode)
            .NotEmpty()
            .Matches("^[0-9]{6}$")
            .WithName("Verification Code");
    }
}
