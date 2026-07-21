using MediatR;
using SAKAN.Application.Features.Properties.DTOs;
using SAKAN.Domain.Enums;

namespace SAKAN.Application.Features.Properties.Commands.UpdateProperty
{
    public class UpdatePropertyCommand : IRequest<PropertyDto>
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public PropertyType PropertyType { get; set; }
        public ContractType ContractType { get; set; }
        public decimal Price { get; set; }
        public decimal Area { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public int FloorsCount { get; set; }
        public int AgeYears { get; set; }
        public PropertyStatus Status { get; set; }
        public string City { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string? PostalCode { get; set; }
        public string? BuildingNumber { get; set; }
        public string? Floor { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public ICollection<Guid> AmenityIds { get; set; } = new List<Guid>();
    }
}
