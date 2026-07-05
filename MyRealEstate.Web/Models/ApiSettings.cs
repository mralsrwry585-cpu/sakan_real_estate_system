using System.ComponentModel.DataAnnotations;

namespace MyRealEstate.Web.Models
{
    public class ApiSettings
    {
        public const string SectionName = "ApiSettings";

        [Required, Url]
        public string BaseUrl { get; set; } = string.Empty;

        public RouteTemplates Routes { get; set; } = new();

        public int TimeoutSeconds { get; set; } = 30;
    }

    public class RouteTemplates
    {
        public string AuthLogin { get; set; } = "api/auth/login";
        public string AuthRegister { get; set; } = "api/auth/register";

        public string Properties { get; set; } = "api/properties";
        public string PropertyById { get; set; } = "api/properties/{0}";
        public string PropertyStatus { get; set; } = "api/properties/{0}/status";
        public string PropertyMedia { get; set; } = "api/properties/{0}/media";
        public string PropertyMediaDelete { get; set; } = "api/properties/{0}/media/{1}";
        public string PropertyMediaCover { get; set; } = "api/properties/{0}/media/cover";
        public string PropertyMediaReorder { get; set; } = "api/properties/{0}/media/reorder";

        public string ViewingRequests { get; set; } = "api/viewingrequests";
        public string ViewingRequestById { get; set; } = "api/viewingrequests/{0}";
        public string ViewingRequestStatus { get; set; } = "api/viewingrequests/{0}/status";

        public string BookingRequests { get; set; } = "api/bookingrequests";
        public string BookingRequestById { get; set; } = "api/bookingrequests/{0}";
        public string BookingRequestStatus { get; set; } = "api/bookingrequests/{0}/status";

        public string Amenities { get; set; } = "api/amenities";
        public string OwnerDashboardStats { get; set; } = "api/analytics/owner/{0}";
    }
}
