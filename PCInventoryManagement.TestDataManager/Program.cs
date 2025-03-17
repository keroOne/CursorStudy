using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PCInventoryManagement.API.Data;
using PCInventoryManagement.API.Models;

namespace PCInventoryManagement.TestDataManager;

public class Program
{
    public static async Task Main(string[] args)
    {
        var services = new ServiceCollection();
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=PCInventoryManagement;Trusted_Connection=True;MultipleActiveResultSets=true"));

        var serviceProvider = services.BuildServiceProvider();
        var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();

        if (args.Length > 0 && (args[0] == "/delete" || args[0] == "/d"))
        {
            await DeleteTestData(dbContext);
            Console.WriteLine("テストデータを削除しました。");
        }
        else
        {
            await InsertTestData(dbContext);
            Console.WriteLine("テストデータを登録しました。");
        }
    }

    private static async Task InsertTestData(ApplicationDbContext dbContext)
    {
        // 拠点データ
        var location1 = new Location { Code = "TYO", Name = "東京本社" };
        var location2 = new Location { Code = "OSK", Name = "大阪支社" };
        dbContext.Locations.AddRange(location1, location2);
        await dbContext.SaveChangesAsync();

        // OS種別データ
        var osType1 = new OSType { Name = "Windows 10" };
        var osType2 = new OSType { Name = "Windows 11" };
        dbContext.OSTypes.AddRange(osType1, osType2);
        await dbContext.SaveChangesAsync();

        // ユーザーデータ
        var user1 = new User
        {
            Name = "山田太郎",
            ADAccount = "yamada.taro",
            DisplayName = "山田 太郎",
            IsActive = true,
            LocationId = location1.Id,
            CreatedAt = DateTime.Now
        };
        var user2 = new User
        {
            Name = "鈴木花子",
            ADAccount = "suzuki.hanako",
            DisplayName = "鈴木 花子",
            IsActive = true,
            LocationId = location2.Id,
            CreatedAt = DateTime.Now
        };
        dbContext.Users.AddRange(user1, user2);
        await dbContext.SaveChangesAsync();

        // PCデータ
        var pc1 = new PC
        {
            ManagementNumber = "PC001",
            ModelName = "ThinkPad X1",
            OSTypeId = osType1.Id,
            UserId = user1.Id,
            CreatedAt = DateTime.Now
        };
        var pc2 = new PC
        {
            ManagementNumber = "PC002",
            ModelName = "ThinkPad X1",
            OSTypeId = osType2.Id,
            UserId = user2.Id,
            CreatedAt = DateTime.Now
        };
        dbContext.PCs.AddRange(pc1, pc2);
        await dbContext.SaveChangesAsync();
    }

    private static async Task DeleteTestData(ApplicationDbContext dbContext)
    {
        dbContext.PCs.RemoveRange(dbContext.PCs);
        await dbContext.SaveChangesAsync();

        dbContext.Users.RemoveRange(dbContext.Users);
        await dbContext.SaveChangesAsync();

        dbContext.OSTypes.RemoveRange(dbContext.OSTypes);
        await dbContext.SaveChangesAsync();

        dbContext.Locations.RemoveRange(dbContext.Locations);
        await dbContext.SaveChangesAsync();
    }
}
