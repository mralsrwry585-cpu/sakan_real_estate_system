using Microsoft.EntityFrameworkCore;
using SAKAN.Domain.Entities;

namespace SAKAN.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<User> Users { get; }
        DbSet<Tenant> Tenants { get; }
        DbSet<Owner> Owners { get; }
        DbSet<Property> Properties { get; }
        DbSet<Address> Addresses { get; }
        DbSet<PropertyMedia> PropertyMedia { get; }
        DbSet<Amenity> Amenities { get; }
        DbSet<PropertyAmenity> PropertyAmenities { get; }
        DbSet<ViewingRequest> ViewingRequests { get; }
        DbSet<BookingRequest> BookingRequests { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
