namespace MyRealEstate.Web.Models.Api
{
    /// <summary>Mirror of SAKAN.Application.Features.Analytics.DTOs.OwnerDashboardStatsDto.</summary>
    public class OwnerDashboardStatsDto
    {
        public int TotalProperties { get; set; }
        public int ActiveProperties { get; set; }
        public int PendingProperties { get; set; }
        public int ReservedProperties { get; set; }
        public int RentedProperties { get; set; }
        public int TotalViews { get; set; }
        public int TotalViewingRequests { get; set; }
        public int PendingViewingRequests { get; set; }
        public int TotalBookingRequests { get; set; }
        public int PendingBookingRequests { get; set; }
        public int ConfirmedBookings { get; set; }
        public int TotalMediaItems { get; set; }
        public decimal AveragePrice { get; set; }
        public decimal TotalRevenue { get; set; }
        public int NewThisMonth { get; set; }
        public int ViewsThisMonth { get; set; }
        public ICollection<PropertyStatusBreakdown> StatusBreakdown { get; set; } = new List<PropertyStatusBreakdown>();
        public ICollection<MonthlyStats> MonthlyStats { get; set; } = new List<MonthlyStats>();
    }

    /// <summary>Mirror of SAKAN.Application.Features.Analytics.DTOs.PropertyStatusBreakdown.</summary>
    public class PropertyStatusBreakdown
    {
        public PropertyStatus Status { get; set; }
        public string StatusName { get; set; } = string.Empty;
        public int Count { get; set; }
    }

    /// <summary>Mirror of SAKAN.Application.Features.Analytics.DTOs.MonthlyStats.</summary>
    public class MonthlyStats
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int PropertiesAdded { get; set; }
        public int Views { get; set; }
        public int Bookings { get; set; }
    }

    /// <summary>Query for GET api/Analytics/owner/&#123;ownerId&#125;.</summary>
    public class GetOwnerDashboardStatsQuery
    {
        public Guid OwnerId { get; set; }
    }
}
