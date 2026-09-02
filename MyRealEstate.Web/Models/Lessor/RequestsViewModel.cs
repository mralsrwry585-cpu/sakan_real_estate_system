using MyRealEstate.Web.Helpers;
using MyRealEstate.Web.Models.Api;

namespace MyRealEstate.Web.Models.Lessor
{
    /// <summary>
    /// View model for the Viewing Requests screen (Lovable OwnerViewingRequests fidelity).
    /// </summary>
    public class ViewingRequestsViewModel
    {
        public List<StatCardViewModel> Stats { get; set; } = new();
        public List<ViewingRequestRowViewModel> Items { get; set; } = new();
        public List<RequestFilterViewModel> Filters { get; set; } = new();
        public ViewingStatus? ActiveFilter { get; set; }
        public int TotalCount { get; set; }
    }

    public class ViewingRequestRowViewModel
    {
        public Guid Id { get; set; }
        public string Number { get; set; } = string.Empty;
        public string PropertyTitle { get; set; } = string.Empty;
        public string TenantName { get; set; } = string.Empty;
        public DateTime RequestedDate { get; set; }
        public string RequestedTime { get; set; } = string.Empty;
        public string? Note { get; set; }
        public string? OwnerResponseNote { get; set; }
        public ViewingStatus Status { get; set; }
        public string StatusLabel => Status.ToArabic();
        public string Tone => Status.RequestTone();
        public string DateLabel => RequestedDate.FormatDate();
    }

    /// <summary>
    /// View model for the Booking Requests screen (Lovable OwnerRequests fidelity).
    /// </summary>
    public class BookingRequestsViewModel
    {
        public List<StatCardViewModel> Stats { get; set; } = new();
        public List<BookingRequestRowViewModel> Items { get; set; } = new();
        public List<RequestFilterViewModel> Filters { get; set; } = new();
        public BookingStatus? ActiveFilter { get; set; }
        public int TotalCount { get; set; }
    }

    public class BookingRequestRowViewModel
    {
        public Guid Id { get; set; }
        public string Number { get; set; } = string.Empty;
        public Guid PropertyId { get; set; }
        public string PropertyTitle { get; set; } = string.Empty;
        public string TenantName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public int DurationMonths { get; set; }
        public string? Note { get; set; }
        public string? OwnerResponseNote { get; set; }
        public BookingStatus Status { get; set; }
        public string StatusLabel => Status.ToArabic();
        public string Tone => Status.RequestTone();
        public string StartDateLabel => StartDate.FormatDate();
        public string DurationLabel => DurationMonths >= 12 && DurationMonths % 12 == 0
            ? $"{DurationMonths / 12} سنة"
            : $"{DurationMonths} شهر";
    }

    public class RequestFilterViewModel
    {
        public string Label { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int Count { get; set; }
    }
}

