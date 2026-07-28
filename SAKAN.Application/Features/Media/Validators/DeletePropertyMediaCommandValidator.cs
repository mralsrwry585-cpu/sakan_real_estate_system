using FluentValidation;
using SAKAN.Application.Features.Media.Commands.DeletePropertyMedia;

namespace SAKAN.Application.Features.Media.Validators
{
    public class DeletePropertyMediaCommandValidator : AbstractValidator<DeletePropertyMediaCommand>
    {
        public DeletePropertyMediaCommandValidator()
        {
            RuleFor(x => x.PropertyId)
                .NotEmpty()
                .WithMessage("PropertyId is required.");

            RuleFor(x => x.MediaId)
                .NotEmpty()
                .WithMessage("MediaId is required.");
        }
    }
}
