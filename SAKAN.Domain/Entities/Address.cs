namespace SAKAN.Domain.Entities
{
    public class Address
    {
        public Guid Id { get; set; }
        public Guid PropertyId { get; set; }
        public string City { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string? PostalCode { get; set; }
        public string? BuildingNumber { get; set; }
        public string? Floor { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

        // Navigation property
        public Property Property { get; set; } = null!;
    }
}
