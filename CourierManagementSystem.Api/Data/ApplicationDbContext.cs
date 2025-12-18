using CourierManagementSystem.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CourierManagementSystem.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Vehicle> Vehicles { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Delivery> Deliveries { get; set; } = null!;
        public DbSet<DeliveryPointConfiguration> DeliveryPoints { get; set; } = null!;
        public DbSet<DeliveryPointProduct> DeliveryPointProducts { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            SeedAdminUser(modelBuilder);

        }
        private void SeedAdminUser(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Login = "admin",
                    PasswordHash = "$2a$10$z1azzGeYiaHewbX.R5XQb.9WzRldo.ER6S749OswSTtGh.E.FORSG",
                    Name = "Системный администратор",
                    Role = UserRole.admin,
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                }
            );
        }
    }
}
