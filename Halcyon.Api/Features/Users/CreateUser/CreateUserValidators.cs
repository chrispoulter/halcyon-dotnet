using FluentValidation;
using Halcyon.Api.Common.Validation;

namespace Halcyon.Api.Features.Users.CreateUser;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator(TimeProvider timeProvider)
    {
        RuleFor(x => x.EmailAddress)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(255)
            .WithName("Email Address");

        RuleFor(x => x.Password).NotEmpty().MinimumLength(8).MaximumLength(50).WithName("Password");
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50).WithName("First Name");
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(50).WithName("Last Name");
        RuleFor(x => x.DateOfBirth).NotEmpty().InThePast(timeProvider).WithName("Date Of Birth");

        RuleForEach(x => x.Roles)
            .Must(role => Roles.AssignableRoles.Contains(role))
            .WithMessage("Role '{PropertyValue}' is not recognized.")
            .WithName("Roles");
    }
}
