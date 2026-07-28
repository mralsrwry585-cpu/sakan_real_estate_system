using FluentValidation;
using SAKAN.Application.Features.Media.Commands.ReorderMedia;

namespace SAKAN.Application.Features.Media.Validators
{
    public class ReorderMediaCommandValidator : AbstractValidator<ReorderMediaCommand>
    {
        public ReorderMediaCommandValidator()
        {
            RuleFor(x => x.PropertyId)
                .NotEmpty()
                .WithMessage("PropertyId is required.");

            RuleFor(x => x.Items)
                .NotEmpty()
                .WithMessage("At least one media item is required for reordering.");

            RuleForEach(x => x.Items)
                .ChildRules(item =>
                {
                    item.RuleFor(i => i.MediaId)
                        .NotEmpty()
                        .WithMessage("MediaId is required for each item.");

                    item.RuleFor(i => i.DisplayOrder)
                        .GreaterThanOrEqualTo(0)
                        .WithMessage("DisplayOrder must be zero or greater.");
                });
        }
    }
}
