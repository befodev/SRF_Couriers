using CourierManagementSystem.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourierManagementSystem.Api.Data
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");

            builder.HasIndex(u => u.Login)
                .IsUnique();

            builder.Property(u => u.Role)
                .HasConversion<string>();
        }
    }
}
