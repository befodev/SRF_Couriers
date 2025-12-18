using CourierManagementSystem.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourierManagementSystem.Api.Data
{
    public class DeliveryPointConfiguration : IEntityTypeConfiguration<DeliveryPoint>
    {
        public void Configure(EntityTypeBuilder<DeliveryPoint> builder)
        {
            builder.ToTable("delivery_points");

            builder.HasIndex(dp => new { dp.DeliveryId, dp.Sequence })
                .IsUnique();

            builder.Property(dp => dp.Latitude)
                .HasPrecision(10, 8);

            builder.Property(dp => dp.Longitude)
                .HasPrecision(11, 8);

            builder.HasMany(dp => dp.DeliveryPointProducts)
                .WithOne(dpp => dpp.DeliveryPoint)
                .HasForeignKey(dpp => dpp.DeliveryPointId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
