using MediatR;
using Microsoft.EntityFrameworkCore;
using SAKAN.Application.Common.Interfaces;
using SAKAN.Application.Features.BookingRequests.DTOs;

namespace SAKAN.Application.Features.BookingRequests.Queries.GetBookingRequestById
{
    public class GetBookingRequestByIdQueryHandler : IRequestHandler<GetBookingRequestByIdQuery, BookingRequestDto>
    {
        private readonly IApplicationDbContext _context;

        public GetBookingRequestByIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BookingRequestDto> Handle(GetBookingRequestByIdQuery request, CancellationToken cancellationToken)
        {
            var bookingRequest = await _context.BookingRequests
                .Include(br => br.Tenant)
                .Include(br => br.Property)
                .Include(br => br.Owner)
                .FirstOrDefaultAsync(br => br.Id == request.Id, cancellationToken);

            if (bookingRequest == null)
                throw new InvalidOperationException("Booking request not found.");

            return new BookingRequestDto
            {
                Id = bookingRequest.Id,
                TenantId = bookingRequest.TenantId,
                TenantName = bookingRequest.Tenant.FullName,
                PropertyId = bookingRequest.PropertyId,
                PropertyTitle = bookingRequest.Property.Title,
                OwnerId = bookingRequest.OwnerId,
                OwnerName = bookingRequest.Owner.FullName,
                BookingNumber = bookingRequest.BookingNumber,
                StartDate = bookingRequest.StartDate,
                DurationMonths = bookingRequest.DurationMonths,
                Note = bookingRequest.Note,
                Status = bookingRequest.Status,
                OwnerResponseNote = bookingRequest.OwnerResponseNote,
                CreatedAt = bookingRequest.CreatedAt,
                UpdatedAt = bookingRequest.UpdatedAt
            };
        }
    }
}
