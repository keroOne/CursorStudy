using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PCInventoryManagement.API.Controllers;
using PCInventoryManagement.API.Data;
using PCInventoryManagement.API.Models;
using PCInventoryManagement.API.Tests.TestData;

namespace PCInventoryManagement.API.Tests.Controllers
{
    public class OSTypesControllerTests
    {
        private readonly OSTypesController _controller;
        private readonly ApplicationDbContext _context;

        public OSTypesControllerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new TestDbContext(options);
            _context.Database.EnsureCreated();

            _controller = new OSTypesController(_context);
        }

        [Fact]
        public async Task GetOSTypes_ReturnsAllOSTypes()
        {
            // Act
            var result = await _controller.GetOSTypes();

            // Assert
            var osTypes = Assert.IsType<List<OSType>>(result.Value);
            Assert.Equal(2, osTypes.Count);
        }

        [Fact]
        public async Task GetOSType_WithValidId_ReturnsOSType()
        {
            // Arrange
            var id = 1;

            // Act
            var result = await _controller.GetOSType(id);

            // Assert
            var osType = Assert.IsType<OSType>(result.Value);
            Assert.Equal(id, osType.Id);
        }

        [Fact]
        public async Task GetOSType_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var id = 999;

            // Act
            var result = await _controller.GetOSType(id);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateOSType_WithValidData_ReturnsCreatedOSType()
        {
            // Arrange
            var osType = new OSType
            {
                Name = "Windows 12",
                IsDeleted = false
            };

            // Act
            var result = await _controller.CreateOSType(osType);

            // Assert
            var createdOSType = Assert.IsType<OSType>(result.Value);
            Assert.Equal(osType.Name, createdOSType.Name);
        }

        [Fact]
        public async Task UpdateOSType_WithValidData_ReturnsNoContent()
        {
            // Arrange
            var id = 1;
            var osType = new OSType
            {
                Id = id,
                Name = "Windows 10 Updated",
                IsDeleted = false
            };

            // Act
            var result = await _controller.UpdateOSType(id, osType);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteOSType_WithValidId_ReturnsNoContent()
        {
            // Arrange
            var id = 1;

            // Act
            var result = await _controller.DeleteOSType(id);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteOSType_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var id = 999;

            // Act
            var result = await _controller.DeleteOSType(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
} 