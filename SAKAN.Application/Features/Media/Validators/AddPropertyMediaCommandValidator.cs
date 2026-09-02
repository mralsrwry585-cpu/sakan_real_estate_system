using FluentValidation;
using SAKAN.Application.Features.Media.Commands.AddPropertyMedia;

namespace SAKAN.Application.Features.Media.Validators
{
    public class AddPropertyMediaCommandValidator
        : AbstractValidator<AddPropertyMediaCommand>
    {
        public AddPropertyMediaCommandValidator()
        {
            // Property ID
            RuleFor(x => x.PropertyId)
                .NotEmpty()
                .WithMessage("Property ID is required.");

            // Media URL
            RuleFor(x => x.Url)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Media URL is required.")
                .MaximumLength(500)
                .WithMessage("Media URL must not exceed 500 characters.")
                .Must(BeValidUrl)
                .WithMessage("Media URL must be a valid HTTP or HTTPS URL.");

            // Display Order
            RuleFor(x => x.DisplayOrder)
                .GreaterThanOrEqualTo(0)
                .When(x => x.DisplayOrder.HasValue)
                .WithMessage("Display order must be zero or greater.");
        }

        private static bool BeValidUrl(string? url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return false;

            return Uri.TryCreate(
                url,
                UriKind.Absolute,
                out var uri)
                && (uri.Scheme == Uri.UriSchemeHttp
                    || uri.Scheme == Uri.UriSchemeHttps);
        }
    }
}