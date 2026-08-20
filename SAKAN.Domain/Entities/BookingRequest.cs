using SAKAN.Domain.Enums;

namespace SAKAN.Domain.Entities
{
    public class BookingRequest
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public Guid PropertyId { get; set; }
        public Guid OwnerId { get; set; }
        public string BookingNumber { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public int DurationMonths { get; set; }
        public string Note { get; set; } = string.Empty;
        public BookingStatus Status { get; set; }
        public string OwnerResponseNote { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public Tenant Tenant { get; set; } = null!;
        public Property Property { get; set; } = null!;
        public Owner Owner { get; set; } = null!;
    }
}
