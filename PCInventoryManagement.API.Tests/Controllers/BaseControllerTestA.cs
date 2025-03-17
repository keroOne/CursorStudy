using System;
using Microsoft.EntityFrameworkCore;
using PCInventoryManagement.API.Data;
using PCInventoryManagement.API.Models;

namespace PCInventoryManagement.API.Tests.Controllers;

public class BaseControllerTestA : IAsyncLifetime
{
    protected ApplicationDbContext _context = null!;

    public virtual async Task InitializeAsync()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        await SetupTestDataAsync();
    }

    public Task DisposeAsync()
    {
        _context.Dispose();
        return Task.CompletedTask;
    }

    private async Task SetupTestDataAsync()
    {
        // 拠点データ
        var locations = new[]
        {
            new Location { Code = "TYO", Name = "東京本社" },
            new Location { Code = "OSK", Name = "大阪支社" }
        };
        _context.Locations.AddRange(locations);
        await _context.SaveChangesAsync();

        // OS種別データ
        var osTypes = new[]
        {
            new OSType { Name = "Windows 10" },
            new OSType { Name = "Windows 11" }
        };
        _context.OSTypes.AddRange(osTypes);
        await _context.SaveChangesAsync();

        // ユーザーデータ
        var users = new[]
        {
            new User
            {
                Name = "山田太郎",
                ADAccount = "yamada.taro",
                DisplayName = "山田 太郎",
                IsActive = true,
                LocationId = locations[0].Id,
                CreatedAt = DateTime.Now
            },
            new User
            {
                Name = "鈴木花子",
                ADAccount = "suzuki.hanako",
                DisplayName = "鈴木 花子",
                IsActive = true,
                LocationId = locations[1].Id,
                CreatedAt = DateTime.Now
            }
        };
        _context.Users.AddRange(users);
        await _context.SaveChangesAsync();

        // PCデータ
        var pcs = new[]
        {
            new PC
            {
                ManagementNumber = "PC001",
                ModelName = "ThinkPad X1",
                OSTypeId = osTypes[0].Id,
                UserId = users[0].Id,
                CreatedAt = DateTime.Now
            },
            new PC
            {
                ManagementNumber = "PC002",
                ModelName = "ThinkPad X1",
                OSTypeId = osTypes[1].Id,
                UserId = users[1].Id,
                CreatedAt = DateTime.Now
            }
        };
        _context.PCs.AddRange(pcs);
        await _context.SaveChangesAsync();
    }
} 