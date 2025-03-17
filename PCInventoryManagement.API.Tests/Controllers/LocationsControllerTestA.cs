using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCInventoryManagement.API.Controllers;
using PCInventoryManagement.API.Models;

namespace PCInventoryManagement.API.Tests.Controllers;

public class LocationsControllerTestA : BaseControllerTestA
{
    private LocationsController _controller = null!;

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        _controller = new LocationsController(_context);
    }

    [Fact]
    public async Task GetLocations_ReturnsAllLocations()
    {
        // Act
        var result = await _controller.GetLocations();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var locations = Assert.IsAssignableFrom<IEnumerable<Location>>(okResult.Value);
        Assert.Equal(2, locations.Count());
    }

    [Fact]
    public async Task GetLocation_WithValidId_ReturnsLocation()
    {
        // Arrange
        var location = await _context.Locations.FirstAsync();

        // Act
        var result = await _controller.GetLocation(location.Id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedLocation = Assert.IsType<Location>(okResult.Value);
        Assert.Equal(location.Id, returnedLocation.Id);
    }

    [Fact]
    public async Task CreateLocation_WithValidData_ReturnsCreatedLocation()
    {
        // Arrange
        var newLocation = new Location
        {
            Code = "NGY",
            Name = "名古屋支社"
        };

        // Act
        var result = await _controller.CreateLocation(newLocation);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnedLocation = Assert.IsType<Location>(createdResult.Value);
        Assert.Equal(newLocation.Name, returnedLocation.Name);
        Assert.NotEqual(0, returnedLocation.Id);
    }

    [Fact]
    public async Task UpdateLocation_WithValidData_ReturnsNoContent()
    {
        // Arrange
        var location = await _context.Locations.FirstAsync();
        location.Name = "Updated Name";

        // Act
        var result = await _controller.UpdateLocation(location.Id, location);

        // Assert
        Assert.IsType<NoContentResult>(result);
        var updatedLocation = await _context.Locations.FindAsync(location.Id);
        Assert.Equal("Updated Name", updatedLocation!.Name);
    }

    [Fact]
    public async Task DeleteLocation_WithValidId_ReturnsNoContent()
    {
        // Arrange
        var location = await _context.Locations.FirstAsync();

        // Act
        var result = await _controller.DeleteLocation(location.Id);

        // Assert
        Assert.IsType<NoContentResult>(result);
        var deletedLocation = await _context.Locations.FindAsync(location.Id);
        Assert.True(deletedLocation!.IsDeleted);
    }
} 