using FluentValidation;
using SAKAN.Application.Features.Media.Commands.SetCoverMedia;

namespace SAKAN.Application.Features.Media.Validators
{
    public class SetCoverMediaCommandValidator : AbstractValidator<SetCoverMediaCommand>
    {
        public SetCoverMediaCommandValidator()
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
