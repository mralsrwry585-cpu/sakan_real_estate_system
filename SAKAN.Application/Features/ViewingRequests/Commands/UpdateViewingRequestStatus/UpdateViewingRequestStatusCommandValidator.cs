using FluentValidation;

namespace SAKAN.Application.Features.ViewingRequests.Commands.UpdateViewingRequestStatus
{
    public class UpdateViewingRequestStatusCommandValidator : AbstractValidator<UpdateViewingRequestStatusCommand>
    {
        public UpdateViewingRequestStatusCommandValidator()
        {
            RuleFor(v => v.Id)
                .NotEmpty().WithMessage("Viewing request ID is required.");

            RuleFor(v => v.Status)
                .IsInEnum().WithMessage("Status is invalid.");

            RuleFor(v => v.OwnerResponseNote)
                .MaximumLength(1000).WithMessage("Owner response note must not exceed 1000 characters.");
        }
    }
}
