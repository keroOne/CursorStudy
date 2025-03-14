using Microsoft.EntityFrameworkCore;
using PCInventoryManagement.API.Data;
using PCInventoryManagement.API.Models;

namespace PCInventoryManagement.API.Tests.TestData
{
    public class TestDbContext : ApplicationDbContext
    {
        public TestDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // テストデータの設定
            modelBuilder.Entity<OSType>().HasData(
                new OSType { Id = 1, Name = "Windows 10", IsDeleted = false },
                new OSType { Id = 2, Name = "Windows 11", IsDeleted = false }
            );

            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, ADAccount = "user1", DisplayName = "User 1", IsActive = true, IsDeleted = false },
                new User { Id = 2, ADAccount = "user2", DisplayName = "User 2", IsActive = true, IsDeleted = false }
            );

            modelBuilder.Entity<PC>().HasData(
                new PC
                {
                    Id = 1,
                    ManagementNumber = "PC-001",
                    ModelName = "ThinkPad X1",
                    OSTypeId = 1,
                    CurrentUserId = 1,
                    IsDeleted = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new PC
                {
                    Id = 2,
                    ManagementNumber = "PC-002",
                    ModelName = "ThinkPad X1",
                    OSTypeId = 2,
                    CurrentUserId = 2,
                    IsDeleted = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            );
        }
    }
} 