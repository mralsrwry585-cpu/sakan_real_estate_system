using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SAKAN.Domain.Entities;

namespace SAKAN.Domain.Configurations
{
    public class ViewingRequestConfiguration : IEntityTypeConfiguration<ViewingRequest>
    {
        public void Configure(EntityTypeBuilder<ViewingRequest> builder)
        {
            builder.ToTable("ViewingRequests");

            builder.HasKey(vr => vr.Id);

            builder.Property(vr => vr.RequestedTime)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(vr => vr.Note)
                .HasMaxLength(1000);

            builder.Property(vr => vr.Status)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(vr => vr.OwnerResponseNote)
                .HasMaxLength(1000);

            builder.Property(vr => vr.CreatedAt)
                .IsRequired();

            // Relationships
            builder.HasOne(vr => vr.Tenant)
                .WithMany(t => t.ViewingRequests)
                .HasForeignKey(vr => vr.TenantId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(vr => vr.Property)
                .WithMany(p => p.ViewingRequests)
                .HasForeignKey(vr => vr.PropertyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(vr => vr.Owner)
                .WithMany(o => o.ViewingRequests)
                .HasForeignKey(vr => vr.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(vr => vr.TenantId);
            builder.HasIndex(vr => vr.PropertyId);
            builder.HasIndex(vr => vr.OwnerId);
            builder.HasIndex(vr => vr.Status);
            builder.HasIndex(vr => vr.CreatedAt);
        }
    }
}
