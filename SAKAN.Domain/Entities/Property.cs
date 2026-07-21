using SAKAN.Domain.Enums;

namespace SAKAN.Domain.Entities
{
    public class Property
    {
        public Guid Id { get; set; }
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
        public PropertyStatus Status { get; set; }
        public int Views { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public Owner Owner { get; set; } = null!;
        public Address Address { get; set; } = null!;
        public ICollection<PropertyMedia> PropertyMedia { get; set; } = new List<PropertyMedia>();
        public ICollection<PropertyAmenity> PropertyAmenities { get; set; } = new List<PropertyAmenity>();
        public ICollection<ViewingRequest> ViewingRequests { get; set; } = new List<ViewingRequest>();
        public ICollection<BookingRequest> BookingRequests { get; set; } = new List<BookingRequest>();
    }
}
