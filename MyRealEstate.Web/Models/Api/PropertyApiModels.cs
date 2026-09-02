namespace MyRealEstate.Web.Models.Api
{
    /// <summary>Mirror of SAKAN.Application.Features.Properties.DTOs.PropertyListDto.</summary>
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

    /// <summary>Mirror of SAKAN.Application.Features.Properties.DTOs.PropertyDto.</summary>
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

    /// <summary>Mirror of SAKAN.Application.Features.Properties.DTOs.AddressDto.</summary>
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

    /// <summary>Mirror of SAKAN.Application.Features.Properties.DTOs.PropertyMediaDto.</summary>
    public class PropertyMediaDto
    {
        public Guid Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public MediaType MediaType { get; set; }
        public bool IsCover { get; set; }
        public int DisplayOrder { get; set; }
    }

    /// <summary>Mirror of SAKAN.Application.Features.Properties.DTOs.PropertyAmenityDto.</summary>
    public class PropertyAmenityDto
    {
        public Guid Id { get; set; }
        public Guid AmenityId { get; set; }
        public string AmenityName { get; set; } = string.Empty;
        public AmenityCategory Category { get; set; }
    }

    /// <summary>Mirror of SAKAN.Application.Features.Properties.Queries.GetAllProperties.GetAllPropertiesQuery.</summary>
    public class GetAllPropertiesQuery
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

    /// <summary>Mirror of SAKAN.Application.Features.Properties.DTOs.CreatePropertyDto + CreatePropertyCommand fields.</summary>
    public class CreatePropertyCommand
    {
        public Guid OwnerId { get; set; }
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

    /// <summary>Mirror of SAKAN.Application.Features.Properties.DTOs.UpdatePropertyDto + UpdatePropertyCommand fields.</summary>
    public class UpdatePropertyCommand
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

    /// <summary>Mirror of SAKAN.Application.Features.Properties.Commands.UpdatePropertyStatus.UpdatePropertyStatusCommand.</summary>
    public class UpdatePropertyStatusCommand
    {
        public Guid Id { get; set; }
        public PropertyStatus Status { get; set; }
    }

    /// <summary>Mirror of SAKAN.Application.Features.Media.Commands.AddPropertyMedia.AddPropertyMediaCommand.</summary>
    public class AddPropertyMediaCommand
    {
        public Guid PropertyId { get; set; }
        public string Url { get; set; } = string.Empty;
        public MediaType MediaType { get; set; }
        public bool IsCover { get; set; }
        public int? DisplayOrder { get; set; }
    }

    /// <summary>Mirror of SAKAN.Application.Features.Media.Commands.SetCoverMedia.SetCoverMediaCommand.</summary>
    public class SetCoverMediaCommand
    {
        public Guid PropertyId { get; set; }
        public Guid MediaId { get; set; }
    }

    /// <summary>Mirror of SAKAN.Application.Features.Media.Commands.ReorderMedia.MediaOrderItem.</summary>
    public class MediaOrderItem
    {
        public Guid MediaId { get; set; }
        public int DisplayOrder { get; set; }
    }

    /// <summary>Mirror of SAKAN.Application.Features.Media.Commands.ReorderMedia.ReorderMediaCommand.</summary>
    public class ReorderMediaCommand
    {
        public Guid PropertyId { get; set; }
        public List<MediaOrderItem> Items { get; set; } = new();
    }

    /// <summary>Mirror of SAKAN.Application.Features.Media.Queries.GetPropertyMedia.GetPropertyMediaQuery.</summary>
    public class GetPropertyMediaQuery
    {
        public Guid PropertyId { get; set; }
    }
}
