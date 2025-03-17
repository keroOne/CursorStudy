using Microsoft.EntityFrameworkCore;
using PCInventoryManagement.API.Models;

namespace PCInventoryManagement.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<PC> PCs { get; set; }
        public DbSet<OSType> OSTypes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Location> Locations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PC>()
                .HasOne(p => p.OSType)
                .WithMany()
                .HasForeignKey(p => p.OSTypeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PC>()
                .HasOne(p => p.User)
                .WithMany(u => u.PCs)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Location)
                .WithMany(l => l.Users)
                .HasForeignKey(u => u.LocationId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
} 