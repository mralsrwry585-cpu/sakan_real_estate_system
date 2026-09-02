using MediatR;
using Microsoft.EntityFrameworkCore;
using SAKAN.Application.Common.Interfaces;
using SAKAN.Application.Common.Models;
using SAKAN.Application.Features.ViewingRequests.DTOs;

namespace SAKAN.Application.Features.ViewingRequests.Queries.GetAllViewingRequests
{
    public class GetAllViewingRequestsQueryHandler : IRequestHandler<GetAllViewingRequestsQuery, PagedResult<ViewingRequestDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetAllViewingRequestsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<ViewingRequestDto>> Handle(GetAllViewingRequestsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.ViewingRequests
                .Include(vr => vr.Tenant)
                .Include(vr => vr.Property)
                .Include(vr => vr.Owner)
                .AsQueryable();

            // Filtering
            if (request.TenantId.HasValue)
                query = query.Where(vr => vr.TenantId == request.TenantId.Value);

            if (request.OwnerId.HasValue)
                query = query.Where(vr => vr.OwnerId == request.OwnerId.Value);

            if (request.PropertyId.HasValue)
                query = query.Where(vr => vr.PropertyId == request.PropertyId.Value);

            if (request.Status.HasValue)
                query = query.Where(vr => vr.Status == request.Status.Value);

            // Sorting
            query = request.SortBy?.ToLower() switch
            {
                "requesteddate" => request.SortAscending ? query.OrderBy(vr => vr.RequestedDate) : query.OrderByDescending(vr => vr.RequestedDate),
                "status" => request.SortAscending ? query.OrderBy(vr => vr.Status) : query.OrderByDescending(vr => vr.Status),
                "createdat" => request.SortAscending ? query.OrderBy(vr => vr.CreatedAt) : query.OrderByDescending(vr => vr.CreatedAt),
                _ => query.OrderByDescending(vr => vr.CreatedAt)
            };

            // Pagination
            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(vr => new ViewingRequestDto
                {
                    Id = vr.Id,
                    TenantId = vr.TenantId,
                    TenantName = vr.Tenant.FullName,
                    PropertyId = vr.PropertyId,
                    PropertyTitle = vr.Property.Title,
                    OwnerId = vr.OwnerId,
                    OwnerName = vr.Owner.FullName,
                    RequestedDate = vr.RequestedDate,
                    RequestedTime = vr.RequestedTime,
                    Note = vr.Note,
                    Status = vr.Status,
                    OwnerResponseNote = vr.OwnerResponseNote,
                    RespondedAt = vr.RespondedAt,
                    CreatedAt = vr.CreatedAt
                })
                .ToListAsync(cancellationToken);

            return new PagedResult<ViewingRequestDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = request.Page,
                PageSize = request.PageSize
            };
        }
    }
}
