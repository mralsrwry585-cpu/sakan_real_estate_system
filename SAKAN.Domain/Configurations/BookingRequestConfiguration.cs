using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SAKAN.Domain.Entities;

namespace SAKAN.Domain.Configurations
{
    public class BookingRequestConfiguration : IEntityTypeConfiguration<BookingRequest>
    {
        public void Configure(EntityTypeBuilder<BookingRequest> builder)
        {
            builder.ToTable("BookingRequests");

            builder.HasKey(br => br.Id);

            builder.Property(br => br.BookingNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(br => br.Note)
                .HasMaxLength(1000);

            builder.Property(br => br.Status)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(br => br.OwnerResponseNote)
                .HasMaxLength(1000);

            builder.Property(br => br.DurationMonths)
                .IsRequired();

            builder.Property(br => br.CreatedAt)
                .IsRequired();

            builder.Property(br => br.UpdatedAt)
                .IsRequired();

            // Relationships
            builder.HasOne(br => br.Tenant)
                .WithMany(t => t.BookingRequests)
                .HasForeignKey(br => br.TenantId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(br => br.Property)
                .WithMany(p => p.BookingRequests)
                .HasForeignKey(br => br.PropertyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(br => br.Owner)
                .WithMany(o => o.BookingRequests)
                .HasForeignKey(br => br.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(br => br.TenantId);
            builder.HasIndex(br => br.PropertyId);
            builder.HasIndex(br => br.OwnerId);
            builder.HasIndex(br => br.Status);
            builder.HasIndex(br => br.BookingNumber).IsUnique();
            builder.HasIndex(br => br.CreatedAt);
        }
    }
}
