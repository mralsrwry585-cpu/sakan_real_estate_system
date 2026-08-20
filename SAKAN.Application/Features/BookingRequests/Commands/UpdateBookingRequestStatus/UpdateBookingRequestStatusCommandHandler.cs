using MediatR;
using Microsoft.EntityFrameworkCore;
using SAKAN.Application.Common.Interfaces;
using SAKAN.Application.Features.BookingRequests.DTOs;

namespace SAKAN.Application.Features.BookingRequests.Commands.UpdateBookingRequestStatus
{
    public class UpdateBookingRequestStatusCommandHandler : IRequestHandler<UpdateBookingRequestStatusCommand, BookingRequestDto>
    {
        private readonly IApplicationDbContext _context;

        public UpdateBookingRequestStatusCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BookingRequestDto> Handle(UpdateBookingRequestStatusCommand request, CancellationToken cancellationToken)
        {
            var bookingRequest = await _context.BookingRequests
                .Include(br => br.Tenant)
                .Include(br => br.Property)
                .Include(br => br.Owner)
                .FirstOrDefaultAsync(br => br.Id == request.Id, cancellationToken);

            if (bookingRequest == null)
                throw new InvalidOperationException("Booking request not found.");

            bookingRequest.Status = request.Status;
            bookingRequest.OwnerResponseNote = request.OwnerResponseNote ?? string.Empty;
            bookingRequest.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return new BookingRequestDto
            {
                Id = bookingRequest.Id,
                TenantId = bookingRequest.TenantId,
                TenantName = bookingRequest.Tenant?.FullName ?? string.Empty,
                PropertyId = bookingRequest.PropertyId,
                PropertyTitle = bookingRequest.Property?.Title ?? string.Empty,
                OwnerId = bookingRequest.OwnerId,
                OwnerName = bookingRequest.Owner?.FullName ?? string.Empty,
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
