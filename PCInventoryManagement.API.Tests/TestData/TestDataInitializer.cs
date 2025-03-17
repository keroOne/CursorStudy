using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PCInventoryManagement.API.Data;
using PCInventoryManagement.API.Models;

namespace PCInventoryManagement.API.Tests.TestData
{
    public static class TestDataInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            // 既存のデータをクリア
            context.Database.EnsureCreated();
            context.PCs.RemoveRange(context.PCs);
            context.Users.RemoveRange(context.Users);
            context.OSTypes.RemoveRange(context.OSTypes);
            context.Locations.RemoveRange(context.Locations);
            context.SaveChanges();

            // 基本データの作成
            var locations = CreateLocations(context);
            var osTypes = CreateOSTypes(context);
            var users = CreateUsers(context, locations);
            var pcs = CreatePCs(context, osTypes, users);
        }

        private static List<Location> CreateLocations(ApplicationDbContext context)
        {
            var locations = new List<Location>
            {
                new Location { Code = "TYO", Name = "東京本社" },
                new Location { Code = "OSK", Name = "大阪支社" }
            };

            context.Locations.AddRange(locations);
            context.SaveChanges();

            return locations;
        }

        private static List<OSType> CreateOSTypes(ApplicationDbContext context)
        {
            var osTypes = new List<OSType>
            {
                new OSType { Name = "Windows 10" },
                new OSType { Name = "Windows 11" }
            };

            context.OSTypes.AddRange(osTypes);
            context.SaveChanges();

            return osTypes;
        }

        private static List<User> CreateUsers(ApplicationDbContext context, List<Location> locations)
        {
            var users = new List<User>
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

            context.Users.AddRange(users);
            context.SaveChanges();

            return users;
        }

        private static List<PC> CreatePCs(ApplicationDbContext context, List<OSType> osTypes, List<User> users)
        {
            var pcs = new List<PC>
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

            context.PCs.AddRange(pcs);
            context.SaveChanges();

            return pcs;
        }

        public static PC CreateTestPC(ApplicationDbContext context)
        {
            var osType = new OSType { Name = "Windows 10" };
            context.OSTypes.Add(osType);

            var location = new Location { Name = "Test Location", Code = "TEST001" };
            context.Locations.Add(location);
            context.SaveChanges();

            var user = new User { Name = "Test User", ADAccount = "test.user", LocationId = location.Id };
            context.Users.Add(user);
            context.SaveChanges();

            var pc = new PC
            {
                ManagementNumber = "TEST001",
                ModelName = "Test Model",
                OSTypeId = osType.Id,
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow
            };

            context.PCs.Add(pc);
            context.SaveChanges();

            return pc;
        }

        public static OSType CreateTestOSType(ApplicationDbContext context)
        {
            var osType = new OSType { Name = "Test OS" };
            context.OSTypes.Add(osType);
            context.SaveChanges();
            return osType;
        }

        public static User CreateTestUser(ApplicationDbContext context)
        {
            var location = new Location { Name = "Test Location", Code = "TEST001" };
            context.Locations.Add(location);
            context.SaveChanges();

            var user = new User { Name = "Test User", ADAccount = "test.user", LocationId = location.Id };
            context.Users.Add(user);
            context.SaveChanges();
            return user;
        }

        public static Location CreateTestLocation(ApplicationDbContext context)
        {
            var location = new Location { Name = "Test Location", Code = "TEST001" };
            context.Locations.Add(location);
            context.SaveChanges();
            return location;
        }
    }
} 