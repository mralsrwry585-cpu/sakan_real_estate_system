using MediatR;
using Microsoft.EntityFrameworkCore;
using SAKAN.Application.Common.Interfaces;
using SAKAN.Application.Features.ViewingRequests.DTOs;

namespace SAKAN.Application.Features.ViewingRequests.Queries.GetViewingRequestById
{
    public class GetViewingRequestByIdQueryHandler : IRequestHandler<GetViewingRequestByIdQuery, ViewingRequestDto>
    {
        private readonly IApplicationDbContext _context;

        public GetViewingRequestByIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ViewingRequestDto> Handle(GetViewingRequestByIdQuery request, CancellationToken cancellationToken)
        {
            var viewingRequest = await _context.ViewingRequests
                .Include(vr => vr.Tenant)
                .Include(vr => vr.Property)
                .Include(vr => vr.Owner)
                .FirstOrDefaultAsync(vr => vr.Id == request.Id, cancellationToken);

            if (viewingRequest == null)
                throw new InvalidOperationException("Viewing request not found.");

            return new ViewingRequestDto
            {
                Id = viewingRequest.Id,
                TenantId = viewingRequest.TenantId,
                TenantName = viewingRequest.Tenant.FullName,
                PropertyId = viewingRequest.PropertyId,
                PropertyTitle = viewingRequest.Property.Title,
                OwnerId = viewingRequest.OwnerId,
                OwnerName = viewingRequest.Owner.FullName,
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
