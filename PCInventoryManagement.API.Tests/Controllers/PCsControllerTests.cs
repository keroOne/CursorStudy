using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PCInventoryManagement.API.Controllers;
using PCInventoryManagement.API.Data;
using PCInventoryManagement.API.Models;
using PCInventoryManagement.API.Tests.TestData;

namespace PCInventoryManagement.API.Tests.Controllers
{
    public class PCsControllerTests
    {
        private readonly PCsController _controller;
        private readonly ApplicationDbContext _context;

        public PCsControllerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new TestDbContext(options);
            _context.Database.EnsureCreated();

            _controller = new PCsController(_context);
        }

        [Fact]
        public async Task GetPCs_ReturnsAllPCs()
        {
            // Act
            var result = await _controller.GetPCs();

            // Assert
            var pcs = Assert.IsType<List<PC>>(result.Value);
            Assert.Equal(2, pcs.Count);
        }

        [Fact]
        public async Task GetPC_WithValidId_ReturnsPC()
        {
            // Arrange
            var id = 1;

            // Act
            var result = await _controller.GetPC(id);

            // Assert
            var pc = Assert.IsType<PC>(result.Value);
            Assert.Equal(id, pc.Id);
        }

        [Fact]
        public async Task GetPC_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var id = 999;

            // Act
            var result = await _controller.GetPC(id);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreatePC_WithValidData_ReturnsCreatedPC()
        {
            // Arrange
            var pc = new PC
            {
                ManagementNumber = "PC-003",
                ModelName = "ThinkPad X1",
                OSTypeId = 1,
                CurrentUserId = 1
            };

            // Act
            var result = await _controller.CreatePC(pc);

            // Assert
            var createdPC = Assert.IsType<PC>(result.Value);
            Assert.Equal(pc.ManagementNumber, createdPC.ManagementNumber);
            Assert.Equal(pc.ModelName, createdPC.ModelName);
        }

        [Fact]
        public async Task UpdatePC_WithValidData_ReturnsNoContent()
        {
            // Arrange
            var id = 1;
            var pc = new PC
            {
                Id = id,
                ManagementNumber = "PC-001-Updated",
                ModelName = "ThinkPad X1",
                OSTypeId = 1,
                CurrentUserId = 1
            };

            // Act
            var result = await _controller.UpdatePC(id, pc);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeletePC_WithValidId_ReturnsNoContent()
        {
            // Arrange
            var id = 1;

            // Act
            var result = await _controller.DeletePC(id);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeletePC_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var id = 999;

            // Act
            var result = await _controller.DeletePC(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
} 