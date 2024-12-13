using eCommerceApp.Application.Dtos.Identity;
using FluentValidation;

namespace eCommerceApp.Application.Validations.Authentication;

public class CreateUserValidator : AbstractValidator<CreateUser>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.FullName).NotEmpty().WithMessage("Full Name is required");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email address");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .Matches(@"[A-Z]").WithMessage("password must contain at least one upper case letter.")
            .Matches(@"[a-z]").WithMessage("password must contain at least one lower case letter.")
            .Matches(@"[\d]").WithMessage(" password must contain at least one digit.")
            .Matches(@"[\W]").WithMessage("password must contain at least one special character.");


        RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage("Passwords do not match");
    }
}
