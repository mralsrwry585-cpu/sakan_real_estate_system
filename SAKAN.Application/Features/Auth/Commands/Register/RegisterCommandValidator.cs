using FluentValidation;

namespace SAKAN.Application.Features.Auth.Commands.Register
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(v => v.FullName)
                .NotEmpty().WithMessage("Full name is required.")
                .MaximumLength(200).WithMessage("Full name must not exceed 200 characters.");

            RuleFor(v => v.Mobile)
                .NotEmpty().WithMessage("Mobile number is required.")
                .MaximumLength(20).WithMessage("Mobile number must not exceed 20 characters.")
                .Matches(@"^\+?[0-9\s\-\(\)]+$").WithMessage("Mobile number format is invalid.");

            RuleFor(v => v.NationalId)
                .NotEmpty().WithMessage("National ID is required.")
                .MaximumLength(20).WithMessage("National ID must not exceed 20 characters.");

            RuleFor(v => v.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email format is invalid.")
                .MaximumLength(200).WithMessage("Email must not exceed 200 characters.");

            RuleFor(v => v.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.")
                .MaximumLength(100).WithMessage("Password must not exceed 100 characters.");

            RuleFor(v => v.Role)
                .IsInEnum().WithMessage("Role is invalid.");
        }
    }
}
