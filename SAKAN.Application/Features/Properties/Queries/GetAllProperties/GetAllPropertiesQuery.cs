using MediatR;
using SAKAN.Application.Common.Models;
using SAKAN.Application.Features.Properties.DTOs;
using SAKAN.Domain.Enums;

namespace SAKAN.Application.Features.Properties.Queries.GetAllProperties
{
    public class GetAllPropertiesQuery : IRequest<PagedResult<PropertyListDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public Guid? OwnerId { get; set; }
        public string? SearchTerm { get; set; }
        public PropertyType? PropertyType { get; set; }
        public ContractType? ContractType { get; set; }
        public PropertyStatus? Status { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? MinBedrooms { get; set; }
        public int? MaxBedrooms { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        public string? SortBy { get; set; }
        public bool SortAscending { get; set; } = false;
    }
}
