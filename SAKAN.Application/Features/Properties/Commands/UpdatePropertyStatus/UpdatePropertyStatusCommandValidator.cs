using FluentValidation;

namespace SAKAN.Application.Features.Properties.Commands.UpdatePropertyStatus
{
    public class UpdatePropertyStatusCommandValidator : AbstractValidator<UpdatePropertyStatusCommand>
    {
        public UpdatePropertyStatusCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Property ID is required.");

            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage("Invalid property status value.");
        }
    }
}
