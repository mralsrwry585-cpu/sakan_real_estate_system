using SAKAN.Domain.Enums;

namespace SAKAN.Domain.Entities
{
    public class ViewingRequest
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public Guid PropertyId { get; set; }
        public Guid OwnerId { get; set; }
        public DateTime RequestedDate { get; set; }
        public string RequestedTime { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public ViewingStatus Status { get; set; }
        public string OwnerResponseNote { get; set; } = string.Empty;
        public DateTime RespondedAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public Tenant Tenant { get; set; } = null!;
        public Property Property { get; set; } = null!;
        public Owner Owner { get; set; } = null!;
    }
}
