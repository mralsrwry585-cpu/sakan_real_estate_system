using MediatR;
using SAKAN.Application.Features.BookingRequests.DTOs;
using SAKAN.Domain.Enums;

namespace SAKAN.Application.Features.BookingRequests.Commands.UpdateBookingRequestStatus
{
    public class UpdateBookingRequestStatusCommand : IRequest<BookingRequestDto>
    {
        public Guid Id { get; set; }
        public BookingStatus Status { get; set; }
        public string? OwnerResponseNote { get; set; }
    }
}
