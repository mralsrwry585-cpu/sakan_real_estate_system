namespace MyRealEstate.Web.Models.Api
{
    /// <summary>Mirror of SAKAN.Application.Features.Amenities.DTOs.AmenityDto.</summary>
    public class AmenityDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public AmenityCategory Category { get; set; }
        public bool IsActive { get; set; }
    }

    /// <summary>Mirror of SAKAN.Application.Features.Amenities.DTOs.AmenityGroupDto.</summary>
    public class AmenityGroupDto
    {
        public AmenityCategory Category { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public ICollection<AmenityDto> Amenities { get; set; } = new List<AmenityDto>();
    }

    /// <summary>Query for GET api/Amenities — mirrors GetAllAmenitiesQuery (includeInactive=false default).</summary>
    public class GetAllAmenitiesQuery
    {
        public bool IncludeInactive { get; set; }
    }
}
