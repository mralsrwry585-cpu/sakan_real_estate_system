using SAKAN.Domain.Enums;

namespace SAKAN.Domain.Entities
{
    public class Owner : User
    {
        public ICollection<Property> Properties { get; set; } = new List<Property>();
        public ICollection<ViewingRequest> ViewingRequests { get; set; } = new List<ViewingRequest>();
        public ICollection<BookingRequest> BookingRequests { get; set; } = new List<BookingRequest>();
    }
}
