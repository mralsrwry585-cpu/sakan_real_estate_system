using MediatR;
using Microsoft.EntityFrameworkCore;
using SAKAN.Application.Common.Interfaces;
using SAKAN.Application.Features.Amenities.DTOs;
using SAKAN.Domain.Enums;

namespace SAKAN.Application.Features.Amenities.Queries.GetAllAmenities
{
    public class GetAllAmenitiesQueryHandler : IRequestHandler<GetAllAmenitiesQuery, IReadOnlyList<AmenityGroupDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetAllAmenitiesQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<AmenityGroupDto>> Handle(GetAllAmenitiesQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Amenities.AsQueryable();

            if (!request.IncludeInactive)
                query = query.Where(a => a.IsActive);

            var amenities = await query
                .OrderBy(a => a.Category)
                .ThenBy(a => a.Name)
                .ToListAsync(cancellationToken);

            var groups = amenities
                .GroupBy(a => a.Category)
                .Select(g => new AmenityGroupDto
                {
                    Category = g.Key,
                    CategoryName = GetCategoryName(g.Key),
                    Amenities = g.Select(a => new AmenityDto
                    {
                        Id = a.Id,
                        Name = a.Name,
                        Category = a.Category,
                        IsActive = a.IsActive
                    }).ToList()
                })
                .ToList();

            return groups;
        }

        private static string GetCategoryName(AmenityCategory category)
        {
            return category switch
            {
                AmenityCategory.Interior => "داخلي",
                AmenityCategory.Exterior => "خارجي",
                AmenityCategory.Services => "خدمات",
                AmenityCategory.Security => "الأمان",
                AmenityCategory.Technology => "تقني",
                _ => category.ToString()
            };
        }
    }
}
