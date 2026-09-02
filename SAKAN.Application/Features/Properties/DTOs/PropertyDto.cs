using SAKAN.Domain.Enums;

namespace SAKAN.Application.Features.Properties.DTOs
{
    public class PropertyDto
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        public string OwnerName { get; set; } = string.Empty;
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
        public int Views { get; set; }
        public int FavoritesCount { get; set; }
        public int ViewingRequestsCount { get; set; }
        public int BookingRequestsCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public AddressDto? Address { get; set; }
        public ICollection<PropertyMediaDto> PropertyMedia { get; set; } = new List<PropertyMediaDto>();
        public ICollection<PropertyAmenityDto> PropertyAmenities { get; set; } = new List<PropertyAmenityDto>();
    }

    public class AddressDto
    {
        public Guid Id { get; set; }
        public string City { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string? PostalCode { get; set; }
        public string? BuildingNumber { get; set; }
        public string? Floor { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }

    public class PropertyMediaDto
    {
        public Guid Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public MediaType MediaType { get; set; }
        public bool IsCover { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class PropertyAmenityDto
    {
        public Guid Id { get; set; }
        public Guid AmenityId { get; set; }
        public string AmenityName { get; set; } = string.Empty;
        public AmenityCategory Category { get; set; }
    }

    public class CreatePropertyDto
    {
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

    public class UpdatePropertyDto
    {
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

    public class PropertyListDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public PropertyType PropertyType { get; set; }
        public ContractType ContractType { get; set; }
        public decimal Price { get; set; }
        public decimal Area { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public PropertyStatus Status { get; set; }
        public int Views { get; set; }
        public int FavoritesCount { get; set; }
        public int ViewingRequestsCount { get; set; }
        public int BookingRequestsCount { get; set; }
        public string City { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string? CoverImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
