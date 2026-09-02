using MediatR;
using SAKAN.Application.Features.BookingRequests.DTOs;

namespace SAKAN.Application.Features.BookingRequests.Queries.GetBookingRequestById
{
    public class GetBookingRequestByIdQuery : IRequest<BookingRequestDto>
    {
        public Guid Id { get; set; }
    }
}
