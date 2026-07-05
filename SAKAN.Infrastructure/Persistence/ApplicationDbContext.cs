using Microsoft.EntityFrameworkCore;
using SAKAN.Application.Common.Interfaces;
using SAKAN.Domain.Entities;
using SAKAN.Domain.Configurations;

namespace SAKAN.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Tenant> Tenants => Set<Tenant>();
        public DbSet<Owner> Owners => Set<Owner>();
        public DbSet<Property> Properties => Set<Property>();
        public DbSet<Address> Addresses => Set<Address>();
        public DbSet<PropertyMedia> PropertyMedia => Set<PropertyMedia>();
        public DbSet<Amenity> Amenities => Set<Amenity>();
        public DbSet<PropertyAmenity> PropertyAmenities => Set<PropertyAmenity>();
        public DbSet<ViewingRequest> ViewingRequests => Set<ViewingRequest>();
        public DbSet<BookingRequest> BookingRequests => Set<BookingRequest>();

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new PropertyConfiguration());
            modelBuilder.ApplyConfiguration(new AmenityConfiguration());
            modelBuilder.ApplyConfiguration(new ViewingRequestConfiguration());
            modelBuilder.ApplyConfiguration(new BookingRequestConfiguration());

            // Seed data for amenities
            SeedAmenities(modelBuilder);
        }

        private static void SeedAmenities(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Amenity>().HasData(
                new Amenity { Id = Guid.Parse("10000000-0000-0000-0000-000000000001"), Name = "Swimming Pool", Category = Domain.Enums.AmenityCategory.Exterior, IsActive = true },
                new Amenity { Id = Guid.Parse("10000000-0000-0000-0000-000000000002"), Name = "Central AC", Category = Domain.Enums.AmenityCategory.Interior, IsActive = true },
                new Amenity { Id = Guid.Parse("10000000-0000-0000-0000-000000000003"), Name = "Parking", Category = Domain.Enums.AmenityCategory.Services, IsActive = true },
                new Amenity { Id = Guid.Parse("10000000-0000-0000-0000-000000000004"), Name = "Security System", Category = Domain.Enums.AmenityCategory.Security, IsActive = true },
                new Amenity { Id = Guid.Parse("10000000-0000-0000-0000-000000000005"), Name = "Smart Home", Category = Domain.Enums.AmenityCategory.Technology, IsActive = true },
                new Amenity { Id = Guid.Parse("10000000-0000-0000-0000-000000000006"), Name = "Garden", Category = Domain.Enums.AmenityCategory.Exterior, IsActive = true },
                new Amenity { Id = Guid.Parse("10000000-0000-0000-0000-000000000007"), Name = "Furnished", Category = Domain.Enums.AmenityCategory.Interior, IsActive = true },
                new Amenity { Id = Guid.Parse("10000000-0000-0000-0000-000000000008"), Name = "Elevator", Category = Domain.Enums.AmenityCategory.Services, IsActive = true },
                new Amenity { Id = Guid.Parse("10000000-0000-0000-0000-000000000009"), Name = "24/7 Security", Category = Domain.Enums.AmenityCategory.Security, IsActive = true },
                new Amenity { Id = Guid.Parse("10000000-0000-0000-0000-000000000010"), Name = "High Speed Internet", Category = Domain.Enums.AmenityCategory.Technology, IsActive = true }
            );
        }
    }
}
