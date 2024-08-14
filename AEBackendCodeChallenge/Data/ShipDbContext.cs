using AEBackendCodeChallenge.Models;
using Microsoft.EntityFrameworkCore;

namespace AEBackendCodeChallenge.Data
{
    public class ShipDbContext : DbContext 
    {
        public ShipDbContext(DbContextOptions<ShipDbContext> options) : base(options) { }

        // DbSets represent collections of the entities
        public DbSet<User> Users { get; set; }
        public DbSet<Ship> Ships { get; set; }  
        public DbSet<Port> Ports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuring the relationships and seeding data

            // One-to-Many: User -> Ships
            modelBuilder.Entity<User>()
                .HasMany(u => u.Ships)
                .WithOne(s => s.User)
                .HasForeignKey(s => s.UserId);

            // Seed data for Ports
            modelBuilder.Entity<Port>().HasData(
                new Port { Id = 1, Name = "Los Angeles Port", Latitude = 34.0522, Longitude = -118.2437 }, // Example coordinates (Los Angeles)
                new Port { Id = 2, Name = "New York Port", Latitude = 40.7128, Longitude = -74.0060 }, // Example coordinates (New York)
                new Port { Id = 3, Name = "London Port", Latitude = 51.5074, Longitude = -0.1278 }  // Example coordinates (London)
            );

        }

    }
}
