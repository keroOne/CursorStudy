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

        public DbSet<PC> PCs { get; set; } = null!;
        public DbSet<OSType> OSTypes { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<PCLocationHistory> PCLocationHistories { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PC>()
                .HasOne(p => p.OSType)
                .WithMany()
                .HasForeignKey(p => p.OSTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PC>()
                .HasOne(p => p.CurrentUser)
                .WithMany()
                .HasForeignKey(p => p.CurrentUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PCLocationHistory>()
                .HasOne(h => h.PC)
                .WithMany()
                .HasForeignKey(h => h.PCId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PCLocationHistory>()
                .HasOne(h => h.FromUser)
                .WithMany()
                .HasForeignKey(h => h.FromUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PCLocationHistory>()
                .HasOne(h => h.ToUser)
                .WithMany()
                .HasForeignKey(h => h.ToUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
} 