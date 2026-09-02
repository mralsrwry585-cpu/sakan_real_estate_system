using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SAKAN.Domain.Entities;

namespace SAKAN.Domain.Configurations
{
    public class PropertyConfiguration : IEntityTypeConfiguration<Property>
    {
        public void Configure(EntityTypeBuilder<Property> builder)
        {
            builder.ToTable("Properties");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Title)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(p => p.Description)
                .HasMaxLength(2000);

            builder.Property(p => p.PropertyType)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(p => p.ContractType)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(p => p.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.Area)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.Bedrooms)
                .IsRequired();

            builder.Property(p => p.Bathrooms)
                .IsRequired();

            builder.Property(p => p.FloorsCount)
                .IsRequired();

            builder.Property(p => p.AgeYears)
                .IsRequired();

            builder.Property(p => p.Status)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(p => p.Views)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(p => p.CreatedAt)
                .IsRequired();

            // Relationships
            builder.HasOne(p => p.Owner)
                .WithMany(o => o.Properties)
                .HasForeignKey(p => p.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Address)
                .WithOne(a => a.Property)
                .HasForeignKey<Address>(a => a.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(p => p.OwnerId);
            builder.HasIndex(p => p.Status);
            builder.HasIndex(p => p.PropertyType);
            builder.HasIndex(p => p.ContractType);
            builder.HasIndex(p => p.Price);
            builder.HasIndex(p => p.CreatedAt);
        }
    }

    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("Addresses");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.City)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.District)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.Street)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(a => a.PostalCode)
                .HasMaxLength(20);

            builder.Property(a => a.BuildingNumber)
                .HasMaxLength(20);

            builder.Property(a => a.Floor)
                .HasMaxLength(20);

            builder.Property(a => a.Latitude)
                .IsRequired()
                .HasColumnType("decimal(18,8)");

            builder.Property(a => a.Longitude)
                .IsRequired()
                .HasColumnType("decimal(18,8)");

            // Indexes
            builder.HasIndex(a => a.City);
            builder.HasIndex(a => a.District);
        }
    }

    public class PropertyMediaConfiguration : IEntityTypeConfiguration<PropertyMedia>
    {
        public void Configure(EntityTypeBuilder<PropertyMedia> builder)
        {
            builder.ToTable("PropertyMedia");

            builder.HasKey(pm => pm.Id);

            builder.Property(pm => pm.Url)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(pm => pm.MediaType)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(pm => pm.IsCover)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(pm => pm.DisplayOrder)
                .IsRequired()
                .HasDefaultValue(0);

            // Relationships
            builder.HasOne(pm => pm.Property)
                .WithMany(p => p.PropertyMedia)
                .HasForeignKey(pm => pm.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(pm => pm.PropertyId);
            builder.HasIndex(pm => new { pm.PropertyId, pm.IsCover });
        }
    }
}
