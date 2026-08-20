using FluentValidation;

namespace SAKAN.Application.Features.BookingRequests.Commands.UpdateBookingRequestStatus
{
    public class UpdateBookingRequestStatusCommandValidator : AbstractValidator<UpdateBookingRequestStatusCommand>
    {
        public UpdateBookingRequestStatusCommandValidator()
        {
            RuleFor(v => v.Id)
                .NotEmpty().WithMessage("Booking request ID is required.");

            RuleFor(v => v.Status)
                .IsInEnum().WithMessage("Status is invalid.");

            RuleFor(v => v.OwnerResponseNote)
                .MaximumLength(1000).WithMessage("Owner response note must not exceed 1000 characters.");
        }
    }
}
