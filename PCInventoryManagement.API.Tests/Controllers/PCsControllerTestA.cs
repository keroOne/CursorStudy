using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCInventoryManagement.API.Controllers;
using PCInventoryManagement.API.Models;

namespace PCInventoryManagement.API.Tests.Controllers;

public class PCsControllerTestA : BaseControllerTestA
{
    private PCsController _controller = null!;

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        _controller = new PCsController(_context);
    }

    [Fact]
    public async Task GetPCs_ReturnsAllPCs()
    {
        // Act
        var result = await _controller.GetPCs();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var pcs = Assert.IsAssignableFrom<IEnumerable<PC>>(okResult.Value);
        Assert.Equal(2, pcs.Count());
    }

    [Fact]
    public async Task GetPC_WithValidId_ReturnsPC()
    {
        // Arrange
        var pc = await _context.PCs.FirstAsync();

        // Act
        var result = await _controller.GetPC(pc.Id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedPC = Assert.IsType<PC>(okResult.Value);
        Assert.Equal(pc.Id, returnedPC.Id);
    }

    [Fact]
    public async Task CreatePC_WithValidData_ReturnsCreatedPC()
    {
        // Arrange
        var osType = await _context.OSTypes.FirstAsync();
        var user = await _context.Users.FirstAsync();
        var newPC = new PC
        {
            ManagementNumber = "PC003",
            ModelName = "ThinkPad X1",
            OSTypeId = osType.Id,
            UserId = user.Id
        };

        // Act
        var result = await _controller.CreatePC(newPC);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        var returnedPC = Assert.IsType<PC>(createdResult.Value);
        Assert.Equal(newPC.ManagementNumber, returnedPC.ManagementNumber);
        Assert.NotEqual(0, returnedPC.Id);
    }

    [Fact]
    public async Task UpdatePC_WithValidData_ReturnsNoContent()
    {
        // Arrange
        var pc = await _context.PCs.FirstAsync();
        pc.ModelName = "Updated Model";

        // Act
        var result = await _controller.UpdatePC(pc.Id, pc);

        // Assert
        Assert.IsType<NoContentResult>(result);
        var updatedPC = await _context.PCs.FindAsync(pc.Id);
        Assert.Equal("Updated Model", updatedPC!.ModelName);
    }

    [Fact]
    public async Task DeletePC_WithValidId_ReturnsNoContent()
    {
        // Arrange
        var pc = await _context.PCs.FirstAsync();

        // Act
        var result = await _controller.DeletePC(pc.Id);

        // Assert
        Assert.IsType<NoContentResult>(result);
        var deletedPC = await _context.PCs.FindAsync(pc.Id);
        Assert.True(deletedPC!.IsDeleted);
    }
} 