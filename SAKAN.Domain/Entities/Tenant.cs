using SAKAN.Domain.Enums;

namespace SAKAN.Domain.Entities
{
    public class Tenant : User
    {
        public ICollection<ViewingRequest> ViewingRequests { get; set; } = new List<ViewingRequest>();
        public ICollection<BookingRequest> BookingRequests { get; set; } = new List<BookingRequest>();
    }
}
