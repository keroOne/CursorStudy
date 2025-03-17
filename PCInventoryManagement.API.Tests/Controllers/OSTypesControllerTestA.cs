using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCInventoryManagement.API.Controllers;
using PCInventoryManagement.API.Models;

namespace PCInventoryManagement.API.Tests.Controllers;

public class OSTypesControllerTestA : BaseControllerTestA
{
    private OSTypesController _controller = null!;

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        _controller = new OSTypesController(_context);
    }

    [Fact]
    public async Task GetOSTypes_ReturnsAllOSTypes()
    {
        // Act
        var result = await _controller.GetOSTypes();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var osTypes = Assert.IsAssignableFrom<IEnumerable<OSType>>(okResult.Value);
        Assert.Equal(2, osTypes.Count());
    }

    [Fact]
    public async Task GetOSType_WithValidId_ReturnsOSType()
    {
        // Arrange
        var osType = await _context.OSTypes.FirstAsync();

        // Act
        var result = await _controller.GetOSType(osType.Id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedOSType = Assert.IsType<OSType>(okResult.Value);
        Assert.Equal(osType.Id, returnedOSType.Id);
    }

    [Fact]
    public async Task CreateOSType_WithValidData_ReturnsCreatedOSType()
    {
        // Arrange
        var newOSType = new OSType
        {
            Name = "Windows 12"
        };

        // Act
        var result = await _controller.CreateOSType(newOSType);

        // Assert
        // var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var createdResult = Assert.IsType<ActionResult<OSType>>(result.Result);
        var returnedOSType = Assert.IsType<OSType>(createdResult.Value);
        Assert.Equal(newOSType.Name, returnedOSType.Name);
        Assert.NotEqual(0, returnedOSType.Id);
    }

    [Fact]
    public async Task UpdateOSType_WithValidData_ReturnsNoContent()
    {
        // Arrange
        var osType = await _context.OSTypes.FirstAsync();
        osType.Name = "Updated Name";

        // Act
        var result = await _controller.UpdateOSType(osType.Id, osType);

        // Assert
        Assert.IsType<NoContentResult>(result);
        var updatedOSType = await _context.OSTypes.FindAsync(osType.Id);
        Assert.Equal("Updated Name", updatedOSType!.Name);
    }

    [Fact]
    public async Task DeleteOSType_WithValidId_ReturnsNoContent()
    {
        // Arrange
        var osType = await _context.OSTypes.FirstAsync();

        // Act
        var result = await _controller.DeleteOSType(osType.Id);

        // Assert
        Assert.IsType<NoContentResult>(result);
        var deletedOSType = await _context.OSTypes.FindAsync(osType.Id);
        Assert.True(deletedOSType!.IsDeleted);
    }
} 