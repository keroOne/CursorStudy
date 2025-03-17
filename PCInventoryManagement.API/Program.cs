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

#if DEBUG
            // デバッグビルド時のみテストデータを設定
            await TestDataInitializer.InitializeTestData(app.Services);
#endif
        }

        app.UseHttpsRedirection();
        app.UseCors("AllowVueApp");
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}
