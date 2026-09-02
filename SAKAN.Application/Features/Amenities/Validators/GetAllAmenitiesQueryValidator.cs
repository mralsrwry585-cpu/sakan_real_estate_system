using FluentValidation;
using SAKAN.Application.Features.Amenities.Queries.GetAllAmenities;

namespace SAKAN.Application.Features.Amenities.Validators
{
    public class GetAllAmenitiesQueryValidator : AbstractValidator<GetAllAmenitiesQuery>
    {
        public GetAllAmenitiesQueryValidator()
        {
            // No required fields for this query; it is a globally accessible catalog.
            RuleFor(x => x.IncludeInactive)
                .NotNull()
                .WithMessage("IncludeInactive must be specified.");
        }
    }
}
