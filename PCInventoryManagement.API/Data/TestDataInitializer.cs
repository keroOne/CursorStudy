using Microsoft.EntityFrameworkCore;
using PCInventoryManagement.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PCInventoryManagement.API.Data;

public static class TestDataInitializer
{
    public static async Task InitializeTestData(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<ApplicationDbContext>();
            context.Database.EnsureCreated();

            // 外部キー制約を一時的に無効化
            context.Database.ExecuteSqlRaw("ALTER TABLE PCs NOCHECK CONSTRAINT ALL");
            context.Database.ExecuteSqlRaw("ALTER TABLE Users NOCHECK CONSTRAINT ALL");
            context.Database.ExecuteSqlRaw("ALTER TABLE OSTypes NOCHECK CONSTRAINT ALL");

            // テーブルをクリア
            await context.Database.ExecuteSqlRawAsync("DELETE FROM PCs");
            await context.Database.ExecuteSqlRawAsync("DELETE FROM Users");
            await context.Database.ExecuteSqlRawAsync("DELETE FROM OSTypes");

            // レコード数が0であることを確認
            var pcCount = await context.PCs.CountAsync();
            var userCount = await context.Users.CountAsync();
            var osTypeCount = await context.OSTypes.CountAsync();

            if (pcCount > 0 || userCount > 0 || osTypeCount > 0)
            {
                throw new Exception($"テーブルのクリアに失敗しました。PCs: {pcCount}, Users: {userCount}, OSTypes: {osTypeCount}");
            }

            // Clear existing data
            context.PCs.RemoveRange(context.PCs);
            context.Users.RemoveRange(context.Users);
            context.OSTypes.RemoveRange(context.OSTypes);
            context.Locations.RemoveRange(context.Locations);
            await context.SaveChangesAsync();

            // Check if tables are empty
            if (!context.OSTypes.Any() && !context.Users.Any() && !context.PCs.Any() && !context.Locations.Any())
            {
                // Add Locations
                var locations = new[]
                {
                    new Location { Code = "SOUKO00001", Name = "倉庫" },
                    new Location { Code = "HONTEN0001", Name = "本店" },
                    new Location { Code = "TOKYO00001", Name = "東京支店" },
                    new Location { Code = "OSAKA00001", Name = "大阪支店" }
                };
                context.Locations.AddRange(locations);
                await context.SaveChangesAsync();

                // Add OSTypes
                var osTypes = new[]
                {
                    new OSType { Name = "Windows 10 Pro" },
                    new OSType { Name = "Windows 11 Pro" }
                };
                context.OSTypes.AddRange(osTypes);
                await context.SaveChangesAsync();

                // Add Users
                var users = new[]
                {
                    new User { Name = "山田太郎", ADAccount = "yamada.taro", LocationId = locations[1].Id },  // 本店
                    new User { Name = "鈴木花子", ADAccount = "suzuki.hanako", LocationId = locations[2].Id },  // 東京支店
                    new User { Name = "佐藤次郎", ADAccount = "sato.jiro", LocationId = locations[3].Id }  // 大阪支店
                };
                context.Users.AddRange(users);
                await context.SaveChangesAsync();

                // Add PCs
                var pcs = new[]
                {
                    new PC { ManagementNumber = "PC001", ModelName = "ThinkPad X1", OSTypeId = osTypes[0].Id, UserId = users[0].Id },
                    new PC { ManagementNumber = "PC002", ModelName = "ThinkPad X2", OSTypeId = osTypes[1].Id, UserId = users[1].Id },
                    new PC { ManagementNumber = "PC003", ModelName = "ThinkPad X3", OSTypeId = osTypes[0].Id, UserId = users[2].Id }
                };
                context.PCs.AddRange(pcs);
                await context.SaveChangesAsync();
            }

            // 外部キー制約を再度有効化
            await context.Database.ExecuteSqlRawAsync("ALTER TABLE PCs CHECK CONSTRAINT ALL");
            await context.Database.ExecuteSqlRawAsync("ALTER TABLE Users CHECK CONSTRAINT ALL");
            await context.Database.ExecuteSqlRawAsync("ALTER TABLE OSTypes CHECK CONSTRAINT ALL");
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "テストデータの初期化中にエラーが発生しました。");
            throw;
        }
    }

    public static void Initialize(ApplicationDbContext context)
    {
        if (context.PCs.Any())
        {
            return;
        }

        var osTypes = new List<OSType>
        {
            new OSType { Name = "Windows 10" },
            new OSType { Name = "Windows 11" },
            new OSType { Name = "macOS" }
        };

        context.OSTypes.AddRange(osTypes);
        context.SaveChanges();

        var locations = new List<Location>
        {
            new Location { Name = "東京オフィス", Code = "TOKYO001" },
            new Location { Name = "大阪オフィス", Code = "OSAKA001" },
            new Location { Name = "名古屋オフィス", Code = "NAGOYA01" }
        };

        context.Locations.AddRange(locations);
        context.SaveChanges();

        var users = new List<User>
        {
            new User { Name = "山田太郎", ADAccount = "yamada.taro", LocationId = locations[0].Id },
            new User { Name = "鈴木花子", ADAccount = "suzuki.hanako", LocationId = locations[1].Id },
            new User { Name = "佐藤次郎", ADAccount = "sato.jiro", LocationId = locations[2].Id }
        };

        context.Users.AddRange(users);
        context.SaveChanges();

        var pcs = new List<PC>
        {
            new PC
            {
                ManagementNumber = "PC001",
                ModelName = "ThinkPad X1",
                OSTypeId = osTypes[0].Id,
                UserId = users[0].Id,
                CreatedAt = DateTime.UtcNow
            },
            new PC
            {
                ManagementNumber = "PC002",
                ModelName = "MacBook Pro",
                OSTypeId = osTypes[2].Id,
                UserId = users[1].Id,
                CreatedAt = DateTime.UtcNow
            },
            new PC
            {
                ManagementNumber = "PC003",
                ModelName = "Surface Pro",
                OSTypeId = osTypes[1].Id,
                UserId = users[2].Id,
                CreatedAt = DateTime.UtcNow
            }
        };

        context.PCs.AddRange(pcs);
        context.SaveChanges();
    }
} 