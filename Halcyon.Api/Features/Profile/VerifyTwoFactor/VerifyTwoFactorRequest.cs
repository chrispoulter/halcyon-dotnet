using FluentValidation;
using Halcyon.Api.Features.Account.LoginWithTwoFactor;

namespace Halcyon.Api.Features.Profile.VerifyTwoFactor;

public record VerifyTwoFactorRequest(string Code);

public class VerifyTwoFactorRequestValidator : AbstractValidator<VerifyTwoFactorRequest>
{
    public VerifyTwoFactorRequestValidator()
    {
        RuleFor(x => x.Code).NotEmpty().Matches("^[0-9]{6}$").WithName("Authenticator Code");
    }
}
