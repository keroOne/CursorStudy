using Microsoft.EntityFrameworkCore;
using PCInventoryManagement.API.Data;
using PCInventoryManagement.API.Models;

namespace PCInventoryManagement.API;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Add DbContext
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        // Add CORS
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowVueApp",
                builder =>
                {
                    builder.WithOrigins("http://localhost:5173", "http://localhost:5174") // Vue.jsの開発サーバーのポート
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseCors("AllowVueApp");
        app.UseAuthorization();
        app.MapControllers();

        // テストデータの初期化
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<ApplicationDbContext>();
                context.Database.EnsureCreated();

                // 外部キー制約を一時的に無効化
                context.Database.ExecuteSqlRaw("ALTER TABLE PCs NOCHECK CONSTRAINT ALL");
                context.Database.ExecuteSqlRaw("ALTER TABLE Users NOCHECK CONSTRAINT ALL");
                context.Database.ExecuteSqlRaw("ALTER TABLE OSTypes NOCHECK CONSTRAINT ALL");

                // テーブルをTruncate
                // await context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE PCs");
                await context.Database.ExecuteSqlRawAsync("DELETE FROM PCs");
                await context.Database.ExecuteSqlRawAsync("DELETE FROM Users");
                await context.Database.ExecuteSqlRawAsync("DELETE FROM OSTypes");

                // レコード数が0であることを確認
                var pcCount = await context.PCs.CountAsync();
                var userCount = await context.Users.CountAsync();
                var osTypeCount = await context.OSTypes.CountAsync();

                if (pcCount > 0 || userCount > 0 || osTypeCount > 0)
                {
                    throw new Exception($"テーブルのTruncateに失敗しました。PCs: {pcCount}, Users: {userCount}, OSTypes: {osTypeCount}");
                }

                // OSタイプの追加
                var osTypes = new List<OSType>
                {
                    new OSType { Name = "Windows 10", IsDeleted = false },
                    new OSType { Name = "Windows 11", IsDeleted = false }
                };
                context.OSTypes.AddRange(osTypes);
                context.SaveChanges();

                // ユーザーの追加
                var users = new List<User>
                {
                    new User { ADAccount = "yamada.taro", DisplayName = "山田太郎", IsActive = true, IsDeleted = false },
                    new User { ADAccount = "yamada.hanako", DisplayName = "山田花子", IsActive = true, IsDeleted = false }
                };
                context.Users.AddRange(users);
                context.SaveChanges();

                // PCの追加
                var addedOSTypes = await context.OSTypes.Where(o => !o.IsDeleted).ToListAsync();
                var addedUsers = await context.Users.Where(u => !u.IsDeleted).ToListAsync();

                var pcs = new List<PC>
                {
                    new PC
                    {
                        ManagementNumber = "PC-001",
                        ModelName = "ThinkPad X1 Carbon",
                        OSTypeId = addedOSTypes[0].Id,
                        CurrentUserId = addedUsers[0].Id,
                        IsDeleted = false,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new PC
                    {
                        ManagementNumber = "PC-002",
                        ModelName = "ThinkPad X1 Yoga",
                        OSTypeId = addedOSTypes[1].Id,
                        CurrentUserId = addedUsers[1].Id,
                        IsDeleted = false,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    }
                };
                await context.PCs.AddRangeAsync(pcs);
                await context.SaveChangesAsync();

                // 外部キー制約を再度有効化
                await context.Database.ExecuteSqlRawAsync("ALTER TABLE PCs CHECK CONSTRAINT ALL");
                await context.Database.ExecuteSqlRawAsync("ALTER TABLE Users CHECK CONSTRAINT ALL");
                await context.Database.ExecuteSqlRawAsync("ALTER TABLE OSTypes CHECK CONSTRAINT ALL");
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "テストデータの初期化中にエラーが発生しました。");
            }
        }

        app.Run();
    }
}
