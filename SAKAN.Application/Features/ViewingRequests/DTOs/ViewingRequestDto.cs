using SAKAN.Domain.Enums;

namespace SAKAN.Application.Features.ViewingRequests.DTOs
{
    public class ViewingRequestDto
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public string TenantName { get; set; } = string.Empty;
        public Guid PropertyId { get; set; }
        public string PropertyTitle { get; set; } = string.Empty;
        public Guid OwnerId { get; set; }
        public string OwnerName { get; set; } = string.Empty;
        public DateTime RequestedDate { get; set; }
        public string RequestedTime { get; set; } = string.Empty;
        public string? Note { get; set; }
        public ViewingStatus Status { get; set; }
        public string? OwnerResponseNote { get; set; }
        public DateTime? RespondedAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateViewingRequestDto
    {
        public Guid PropertyId { get; set; }
        public DateTime RequestedDate { get; set; }
        public string RequestedTime { get; set; } = string.Empty;
        public string? Note { get; set; }
    }

    public class UpdateViewingRequestStatusDto
    {
        public ViewingStatus Status { get; set; }
        public string? OwnerResponseNote { get; set; }
    }
}
