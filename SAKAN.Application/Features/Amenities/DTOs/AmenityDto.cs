using SAKAN.Domain.Enums;

namespace SAKAN.Application.Features.Amenities.DTOs
{
    public class AmenityDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public AmenityCategory Category { get; set; }
        public bool IsActive { get; set; }
    }

    public class AmenityGroupDto
    {
        public AmenityCategory Category { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public ICollection<AmenityDto> Amenities { get; set; } = new List<AmenityDto>();
    }
}
