using SAKAN.Domain.Enums;

namespace SAKAN.Application.Features.BookingRequests.DTOs
{
    public class BookingRequestDto
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public string TenantName { get; set; } = string.Empty;
        public Guid PropertyId { get; set; }
        public string PropertyTitle { get; set; } = string.Empty;
        public Guid OwnerId { get; set; }
        public string OwnerName { get; set; } = string.Empty;
        public string BookingNumber { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public int DurationMonths { get; set; }
        public string? Note { get; set; }
        public BookingStatus Status { get; set; }
        public string? OwnerResponseNote { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateBookingRequestDto
    {
        public Guid PropertyId { get; set; }
        public DateTime StartDate { get; set; }
        public int DurationMonths { get; set; }
        public string? Note { get; set; }
    }

    public class UpdateBookingRequestStatusDto
    {
        public BookingStatus Status { get; set; }
        public string? OwnerResponseNote { get; set; }
    }
}
