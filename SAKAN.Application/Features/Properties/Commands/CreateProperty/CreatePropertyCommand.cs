using MediatR;
using SAKAN.Application.Features.Properties.DTOs;

namespace SAKAN.Application.Features.Properties.Commands.CreateProperty
{
    public class CreatePropertyCommand : IRequest<PropertyDto>
    {
        public Guid OwnerId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Domain.Enums.PropertyType PropertyType { get; set; }
        public Domain.Enums.ContractType ContractType { get; set; }
        public decimal Price { get; set; }
        public decimal Area { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public int FloorsCount { get; set; }
        public int AgeYears { get; set; }
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
