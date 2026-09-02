using MediatR;
using Microsoft.EntityFrameworkCore;
using SAKAN.Application.Common.Interfaces;
using SAKAN.Application.Features.ViewingRequests.DTOs;
using SAKAN.Domain.Enums;

namespace SAKAN.Application.Features.ViewingRequests.Commands.UpdateViewingRequestStatus
{
    public class UpdateViewingRequestStatusCommandHandler : IRequestHandler<UpdateViewingRequestStatusCommand, ViewingRequestDto>
    {
        private readonly IApplicationDbContext _context;

        public UpdateViewingRequestStatusCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ViewingRequestDto> Handle(UpdateViewingRequestStatusCommand request, CancellationToken cancellationToken)
        {
            var viewingRequest = await _context.ViewingRequests
                .Include(vr => vr.Tenant)
                .Include(vr => vr.Property)
                .Include(vr => vr.Owner)
                .FirstOrDefaultAsync(vr => vr.Id == request.Id, cancellationToken);

            if (viewingRequest == null)
                throw new InvalidOperationException("Viewing request not found.");

            viewingRequest.Status = request.Status;
            viewingRequest.OwnerResponseNote = request.OwnerResponseNote ?? string.Empty;
            viewingRequest.RespondedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return new ViewingRequestDto
            {
                Id = viewingRequest.Id,
                TenantId = viewingRequest.TenantId,
                TenantName = viewingRequest.Tenant?.FullName ?? string.Empty,
                PropertyId = viewingRequest.PropertyId,
                PropertyTitle = viewingRequest.Property?.Title ?? string.Empty,
                OwnerId = viewingRequest.OwnerId,
                OwnerName = viewingRequest.Owner?.FullName ?? string.Empty,
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
