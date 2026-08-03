using SAKAN.Domain.Enums;

namespace SAKAN.Domain.Entities
{
    public class Amenity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public AmenityCategory Category { get; set; }
        public bool IsActive { get; set; }

        // Navigation property
        public ICollection<PropertyAmenity> PropertyAmenities { get; set; } = new List<PropertyAmenity>();
    }
}
