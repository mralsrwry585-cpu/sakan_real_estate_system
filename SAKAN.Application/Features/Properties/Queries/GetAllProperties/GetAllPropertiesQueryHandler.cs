using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SAKAN.Application.Common.Interfaces;
using SAKAN.Application.Common.Models;
using SAKAN.Application.Features.Properties.DTOs;

namespace SAKAN.Application.Features.Properties.Queries.GetAllProperties
{
    public class GetAllPropertiesQueryHandler : IRequestHandler<GetAllPropertiesQuery, PagedResult<PropertyListDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAllPropertiesQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagedResult<PropertyListDto>> Handle(GetAllPropertiesQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Properties
                .Include(p => p.Address)
                .Include(p => p.PropertyMedia)
                .Include(p => p.Owner)
                .Include(p => p.ViewingRequests)
                .Include(p => p.BookingRequests)
                .AsQueryable();

            // Apply filtering
            if (request.OwnerId.HasValue)
                query = query.Where(p => p.OwnerId == request.OwnerId.Value);

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var search = request.SearchTerm.ToLower();
                query = query.Where(p => p.Title.ToLower().Contains(search) ||
                                         p.Description.ToLower().Contains(search) ||
                                         p.Address.City.ToLower().Contains(search) ||
                                         p.Address.District.ToLower().Contains(search));
            }

            if (request.PropertyType.HasValue)
                query = query.Where(p => p.PropertyType == request.PropertyType.Value);

            if (request.ContractType.HasValue)
                query = query.Where(p => p.ContractType == request.ContractType.Value);

            if (request.Status.HasValue)
                query = query.Where(p => p.Status == request.Status.Value);

            if (request.MinPrice.HasValue)
                query = query.Where(p => p.Price >= request.MinPrice.Value);

            if (request.MaxPrice.HasValue)
                query = query.Where(p => p.Price <= request.MaxPrice.Value);

            if (request.MinBedrooms.HasValue)
                query = query.Where(p => p.Bedrooms >= request.MinBedrooms.Value);

            if (request.MaxBedrooms.HasValue)
                query = query.Where(p => p.Bedrooms <= request.MaxBedrooms.Value);

            if (!string.IsNullOrWhiteSpace(request.City))
                query = query.Where(p => p.Address.City.ToLower() == request.City.ToLower());

            if (!string.IsNullOrWhiteSpace(request.District))
                query = query.Where(p => p.Address.District.ToLower() == request.District.ToLower());

            // Apply sorting
            query = request.SortBy?.ToLower() switch
            {
                "price" => request.SortAscending ? query.OrderBy(p => p.Price) : query.OrderByDescending(p => p.Price),
                "createdat" => request.SortAscending ? query.OrderBy(p => p.CreatedAt) : query.OrderByDescending(p => p.CreatedAt),
                "area" => request.SortAscending ? query.OrderBy(p => p.Area) : query.OrderByDescending(p => p.Area),
                "bedrooms" => request.SortAscending ? query.OrderBy(p => p.Bedrooms) : query.OrderByDescending(p => p.Bedrooms),
                "views" => request.SortAscending ? query.OrderBy(p => p.Views) : query.OrderByDescending(p => p.Views),
                _ => query.OrderByDescending(p => p.CreatedAt)
            };

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            var dtos = items.Select(p => new PropertyListDto
            {
                Id = p.Id,
                Title = p.Title,
                PropertyType = p.PropertyType,
                ContractType = p.ContractType,
                Price = p.Price,
                Area = p.Area,
                Bedrooms = p.Bedrooms,
                Bathrooms = p.Bathrooms,
                Status = p.Status,
                Views = p.Views,
                FavoritesCount = 0,
                ViewingRequestsCount = p.ViewingRequests?.Count ?? 0,
                BookingRequestsCount = p.BookingRequests?.Count ?? 0,
                City = p.Address?.City ?? string.Empty,
                District = p.Address?.District ?? string.Empty,
                CoverImageUrl = p.PropertyMedia?.FirstOrDefault(pm => pm.IsCover)?.Url,
                CreatedAt = p.CreatedAt
            }).ToList();

            return new PagedResult<PropertyListDto>(dtos, totalCount, request.PageNumber, request.PageSize);
        }
    }
}
