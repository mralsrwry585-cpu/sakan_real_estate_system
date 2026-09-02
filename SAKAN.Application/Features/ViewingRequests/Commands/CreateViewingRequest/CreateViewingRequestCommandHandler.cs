using MediatR;
using Microsoft.EntityFrameworkCore;
using SAKAN.Application.Common.Interfaces;
using SAKAN.Application.Features.ViewingRequests.DTOs;
using SAKAN.Domain.Entities;
using SAKAN.Domain.Enums;

namespace SAKAN.Application.Features.ViewingRequests.Commands.CreateViewingRequest
{
    public class CreateViewingRequestCommandHandler
        : IRequestHandler<CreateViewingRequestCommand, ViewingRequestDto>
    {
        private readonly IApplicationDbContext _context;

        public CreateViewingRequestCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ViewingRequestDto> Handle(
            CreateViewingRequestCommand request,
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
                    "Property is not available for viewing requests.");

            // 2. Check Tenant/User
            var tenant = await _context.Users
                .FirstOrDefaultAsync(
                    u => u.Id == request.TenantId,
                    cancellationToken);

            if (tenant == null)
                throw new InvalidOperationException(
                    "Tenant user not found.");

            // 3. Create Viewing Request
            var viewingRequest = new ViewingRequest
            {
                Id = Guid.NewGuid(),

                PropertyId = request.PropertyId,

                TenantId = request.TenantId,

                OwnerId = property.OwnerId,

                RequestedDate = request.RequestedDate,

                RequestedTime = request.RequestedTime,

                Note = request.Note ?? string.Empty,

                Status = ViewingStatus.Pending,

                CreatedAt = DateTime.UtcNow
            };

            _context.ViewingRequests.Add(viewingRequest);

            await _context.SaveChangesAsync(cancellationToken);

            // 4. Return DTO
            return new ViewingRequestDto
            {
                Id = viewingRequest.Id,

                TenantId = viewingRequest.TenantId,

                TenantName = tenant.FullName ?? string.Empty,

                PropertyId = viewingRequest.PropertyId,

                PropertyTitle = property.Title ?? string.Empty,

                OwnerId = viewingRequest.OwnerId,

                OwnerName = property.Owner?.FullName ?? string.Empty,

                RequestedDate = viewingRequest.RequestedDate,

                RequestedTime = viewingRequest.RequestedTime,

                Note = viewingRequest.Note,

                Status = viewingRequest.Status,

                OwnerResponseNote = viewingRequest.OwnerResponseNote,

                RespondedAt = viewingRequest.RespondedAt,

                CreatedAt = viewingRequest.CreatedAt
            };
        }
    }
}