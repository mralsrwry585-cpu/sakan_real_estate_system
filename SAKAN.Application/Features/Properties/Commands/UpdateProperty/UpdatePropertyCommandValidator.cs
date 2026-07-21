using FluentValidation;

namespace SAKAN.Application.Features.Properties.Commands.UpdateProperty
{
    public class UpdatePropertyCommandValidator : AbstractValidator<UpdatePropertyCommand>
    {
        public UpdatePropertyCommandValidator()
        {
            RuleFor(v => v.Id)
                .NotEmpty().WithMessage("Property ID is required.");

            RuleFor(v => v.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(300).WithMessage("Title must not exceed 300 characters.");

            RuleFor(v => v.Description)
                .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters.");

            RuleFor(v => v.PropertyType)
                .IsInEnum().WithMessage("Invalid property type.");

            RuleFor(v => v.ContractType)
                .IsInEnum().WithMessage("Invalid contract type.");

            RuleFor(v => v.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0.");

            RuleFor(v => v.Area)
                .GreaterThan(0).WithMessage("Area must be greater than 0.");

            RuleFor(v => v.Bedrooms)
                .GreaterThanOrEqualTo(0).WithMessage("Bedrooms must be 0 or greater.");

            RuleFor(v => v.Bathrooms)
                .GreaterThanOrEqualTo(0).WithMessage("Bathrooms must be 0 or greater.");

            RuleFor(v => v.FloorsCount)
                .GreaterThanOrEqualTo(0).WithMessage("Floors count must be 0 or greater.");

            RuleFor(v => v.AgeYears)
                .GreaterThanOrEqualTo(0).WithMessage("Age years must be 0 or greater.");

            RuleFor(v => v.Status)
                .IsInEnum().WithMessage("Invalid property status.");

            RuleFor(v => v.City)
                .NotEmpty().WithMessage("City is required.")
                .MaximumLength(100).WithMessage("City must not exceed 100 characters.");

            RuleFor(v => v.District)
                .NotEmpty().WithMessage("District is required.")
                .MaximumLength(100).WithMessage("District must not exceed 100 characters.");

            RuleFor(v => v.Street)
                .NotEmpty().WithMessage("Street is required.")
                .MaximumLength(200).WithMessage("Street must not exceed 200 characters.");

            RuleFor(v => v.Latitude)
                .InclusiveBetween(-90, 90).WithMessage("Latitude must be between -90 and 90.");

            RuleFor(v => v.Longitude)
                .InclusiveBetween(-180, 180).WithMessage("Longitude must be between -180 and 180.");
        }
    }
}
