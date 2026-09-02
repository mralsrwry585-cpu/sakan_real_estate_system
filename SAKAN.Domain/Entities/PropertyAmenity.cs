namespace SAKAN.Domain.Entities
{
    public class PropertyAmenity
    {
        public Guid Id { get; set; }
        public Guid PropertyId { get; set; }
        public Guid AmenityId { get; set; }

        // Navigation properties
        public Property Property { get; set; } = null!;
        public Amenity Amenity { get; set; } = null!;
    }
}
