using MediatR;
using Microsoft.EntityFrameworkCore;
using SAKAN.Application.Common.Interfaces;
using SAKAN.Application.Features.BookingRequests.DTOs;
using SAKAN.Domain.Entities;
using SAKAN.Domain.Enums;

namespace SAKAN.Application.Features.BookingRequests.Commands.CreateBookingRequest
{
    public class CreateBookingRequestCommandHandler
        : IRequestHandler<CreateBookingRequestCommand, BookingRequestDto>
    {
        private readonly IApplicationDbContext _context;

        public CreateBookingRequestCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BookingRequestDto> Handle(
            CreateBookingRequestCommand request,
            CancellationToken cancellationToken)
        {
            // 1. Check Property
            var property = await _context.Properties
                .Include(p => p.Owner)
                .FirstOrDefaultAsync(
                    p => p.Id == request.PropertyId,
                    cancellationToken);

            if (property == null)
                throw new InvalidOperationException("Property not found.");

            if (property.Status != PropertyStatus.Available)
                throw new InvalidOperationException(
                    "Property is not available for booking.");

            // 2. Check Tenant/User
            // TenantId is supplied from the request body
            // and must exist in Users.Id because of the database FK.
            var tenant = await _context.Users
                .FirstOrDefaultAsync(
                    u => u.Id == request.TenantId,
                    cancellationToken);

            if (tenant == null)
                throw new InvalidOperationException(
                    "Tenant user not found.");

            // 3. Generate unique booking number
            var bookingNumber =
                $"BK-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid():N}"
                .ToUpper()[..20];

            // 4. Create Booking Request
            var bookingRequest = new BookingRequest
            {
                Id = Guid.NewGuid(),

                PropertyId = request.PropertyId,

                TenantId = request.TenantId,

                OwnerId = property.OwnerId,

                BookingNumber = bookingNumber,

                StartDate = request.StartDate,

                DurationMonths = request.DurationMonths,

                Note = request.Note ?? string.Empty,

                Status = BookingStatus.Pending,

                CreatedAt = DateTime.UtcNow,

                UpdatedAt = DateTime.UtcNow
            };

            _context.BookingRequests.Add(bookingRequest);

            await _context.SaveChangesAsync(cancellationToken);

            // 5. Return DTO
            return new BookingRequestDto
            {
                Id = bookingRequest.Id,

                TenantId = bookingRequest.TenantId,

                TenantName = tenant.FullName ?? string.Empty,

                PropertyId = bookingRequest.PropertyId,

                PropertyTitle = property.Title ?? string.Empty,

                OwnerId = bookingRequest.OwnerId,

                OwnerName = property.Owner?.FullName ?? string.Empty,

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