using FluentValidation;

namespace SAKAN.Application.Features.BookingRequests.Commands.CreateBookingRequest
{
    public class CreateBookingRequestCommandValidator
        : AbstractValidator<CreateBookingRequestCommand>
    {
        public CreateBookingRequestCommandValidator()
        {
            // Property
            RuleFor(v => v.PropertyId)
                .NotEmpty()
                .WithMessage("Property ID is required.");

            // Start Date
            RuleFor(v => v.StartDate)
                .Must(date => date != default)
                .WithMessage("Start date is required.")
                .GreaterThanOrEqualTo(DateTime.Today)
                .WithMessage("Start date must be today or a future date.");

            // Duration
            RuleFor(v => v.DurationMonths)
                .InclusiveBetween(1, 60)
                .WithMessage("Duration must be between 1 and 60 months.");

            // Note
            RuleFor(v => v.Note)
                .MaximumLength(1000)
                .WithMessage("Note must not exceed 1000 characters.")
                .Must(note => string.IsNullOrWhiteSpace(note) || !string.IsNullOrWhiteSpace(note))
                .WithMessage("Note cannot contain only whitespace.");
        }
    }
}