namespace MyRealEstate.Web.Models.Api
{
    /// <summary>Mirror of SAKAN.Application.Features.BookingRequests.DTOs.BookingRequestDto.</summary>
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

    /// <summary>Mirror of SAKAN.Application.Features.ViewingRequests.DTOs.ViewingRequestDto.</summary>
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

    /// <summary>Query parameters for GET api/BookingRequests — mirrors GetAllBookingRequestsQuery.</summary>
    public class GetAllBookingRequestsQuery
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public Guid? TenantId { get; set; }
        public Guid? OwnerId { get; set; }
        public Guid? PropertyId { get; set; }
        public BookingStatus? Status { get; set; }
        public string? SortBy { get; set; }
        public bool SortAscending { get; set; } = true;
    }

    /// <summary>Query parameters for GET api/ViewingRequests — mirrors GetAllViewingRequestsQuery.</summary>
    public class GetAllViewingRequestsQuery
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public Guid? TenantId { get; set; }
        public Guid? OwnerId { get; set; }
        public Guid? PropertyId { get; set; }
        public ViewingStatus? Status { get; set; }
        public string? SortBy { get; set; }
        public bool SortAscending { get; set; } = true;
    }

    /// <summary>Body for PUT api/BookingRequests/&#123;id&#125;/status — mirrors UpdateBookingRequestStatusCommand.</summary>
    public class UpdateBookingRequestStatusCommand
    {
        public Guid Id { get; set; }
        public BookingStatus Status { get; set; }
        public string? OwnerResponseNote { get; set; }
    }

    /// <summary>Body for PUT api/ViewingRequests/&#123;id&#125;/status — mirrors UpdateViewingRequestStatusCommand.</summary>
    public class UpdateViewingRequestStatusCommand
    {
        public Guid Id { get; set; }
        public ViewingStatus Status { get; set; }
        public string? OwnerResponseNote { get; set; }
    }
}
