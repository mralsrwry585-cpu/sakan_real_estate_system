using MediatR;
using SAKAN.Application.Features.BookingRequests.DTOs;

namespace SAKAN.Application.Features.BookingRequests.Commands.CreateBookingRequest
{
    public class CreateBookingRequestCommand : IRequest<BookingRequestDto>
    {
        public Guid PropertyId { get; set; }
        public DateTime StartDate { get; set; }

        public Guid TenantId { get; set; }
        public int DurationMonths { get; set; }
        public string? Note { get; set; }
    }
}
