using Microsoft.EntityFrameworkCore;
using PCInventoryManagement.API.Data;
using System;

namespace PCInventoryManagement.API.Tests.Controllers
{
    public abstract class BaseControllerTest : IDisposable
    {
        protected readonly ApplicationDbContext _context;

        protected BaseControllerTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            TestDataInitializer.Initialize(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
} 