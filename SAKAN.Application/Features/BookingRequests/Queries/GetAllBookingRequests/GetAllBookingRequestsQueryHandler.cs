using MediatR;
using Microsoft.EntityFrameworkCore;
using SAKAN.Application.Common.Interfaces;
using SAKAN.Application.Common.Models;
using SAKAN.Application.Features.BookingRequests.DTOs;

namespace SAKAN.Application.Features.BookingRequests.Queries.GetAllBookingRequests
{
    public class GetAllBookingRequestsQueryHandler : IRequestHandler<GetAllBookingRequestsQuery, PagedResult<BookingRequestDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetAllBookingRequestsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<BookingRequestDto>> Handle(GetAllBookingRequestsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.BookingRequests
                .Include(br => br.Tenant)
                .Include(br => br.Property)
                .Include(br => br.Owner)
                .AsQueryable();

            // Filtering
            if (request.TenantId.HasValue)
                query = query.Where(br => br.TenantId == request.TenantId.Value);

            if (request.OwnerId.HasValue)
                query = query.Where(br => br.OwnerId == request.OwnerId.Value);

            if (request.PropertyId.HasValue)
                query = query.Where(br => br.PropertyId == request.PropertyId.Value);

            if (request.Status.HasValue)
                query = query.Where(br => br.Status == request.Status.Value);

            // Sorting
            query = request.SortBy?.ToLower() switch
            {
                "startdate" => request.SortAscending ? query.OrderBy(br => br.StartDate) : query.OrderByDescending(br => br.StartDate),
                "status" => request.SortAscending ? query.OrderBy(br => br.Status) : query.OrderByDescending(br => br.Status),
                "createdat" => request.SortAscending ? query.OrderBy(br => br.CreatedAt) : query.OrderByDescending(br => br.CreatedAt),
                _ => query.OrderByDescending(br => br.CreatedAt)
            };

            // Pagination
            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(br => new BookingRequestDto
                {
                    Id = br.Id,
                    TenantId = br.TenantId,
                    TenantName = br.Tenant.FullName,
                    PropertyId = br.PropertyId,
                    PropertyTitle = br.Property.Title,
                    OwnerId = br.OwnerId,
                    OwnerName = br.Owner.FullName,
                    BookingNumber = br.BookingNumber,
                    StartDate = br.StartDate,
                    DurationMonths = br.DurationMonths,
                    Note = br.Note,
                    Status = br.Status,
                    OwnerResponseNote = br.OwnerResponseNote,
                    CreatedAt = br.CreatedAt,
                    UpdatedAt = br.UpdatedAt
                })
                .ToListAsync(cancellationToken);

            return new PagedResult<BookingRequestDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = request.Page,
                PageSize = request.PageSize
            };
        }
    }
}
