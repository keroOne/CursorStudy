using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using PCInventoryManagement.API.Data;

namespace PCInventoryManagement.API.Tests;

public class TestBase : IAsyncLifetime
{
    protected ApplicationDbContext _context = null!;

    public async Task InitializeAsync()
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "..",
                    "..",
                    "..",
                    "..",
                    "PCInventoryManagement.TestDataManager",
                    "bin",
                    "Debug",
                    "net8.0",
                    "PCInventoryManagement.TestDataManager.exe"
                ),
                UseShellExecute = true
            }
        };
        process.Start();
        await process.WaitForExitAsync();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=PCInventoryManagement;Trusted_Connection=True;MultipleActiveResultSets=true")
            .Options;

        _context = new ApplicationDbContext(options);
    }

    public async Task DisposeAsync()
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "..",
                    "..",
                    "..",
                    "..",
                    "PCInventoryManagement.TestDataManager",
                    "bin",
                    "Debug",
                    "net8.0",
                    "PCInventoryManagement.TestDataManager.exe"
                ),
                Arguments = "/delete",
                UseShellExecute = true
            }
        };
        process.Start();
        await process.WaitForExitAsync();

        _context.Dispose();
    }
} 