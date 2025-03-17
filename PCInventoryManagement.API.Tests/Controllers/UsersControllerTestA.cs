using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCInventoryManagement.API.Controllers;
using PCInventoryManagement.API.Models;

namespace PCInventoryManagement.API.Tests.Controllers;

public class UsersControllerTestA : BaseControllerTestA
{
    private UsersController _controller = null!;

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        _controller = new UsersController(_context);
    }

    [Fact]
    public async Task GetUsers_ReturnsAllUsers()
    {
        // Act
        var result = await _controller.GetUsers();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var users = Assert.IsAssignableFrom<IEnumerable<User>>(okResult.Value);
        Assert.Equal(2, users.Count());
    }

    [Fact]
    public async Task GetUser_WithValidId_ReturnsUser()
    {
        // Arrange
        var user = await _context.Users.FirstAsync();

        // Act
        var result = await _controller.GetUser(user.Id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedUser = Assert.IsType<User>(okResult.Value);
        Assert.Equal(user.Id, returnedUser.Id);
    }

    [Fact]
    public async Task CreateUser_WithValidData_ReturnsCreatedUser()
    {
        // Arrange
        var location = await _context.Locations.FirstAsync();
        var newUser = new User
        {
            Name = "新規ユーザー",
            ADAccount = "new.user",
            DisplayName = "新規 ユーザー",
            IsActive = true,
            LocationId = location.Id
        };

        // Act
        var result = await _controller.CreateUser(newUser);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnedUser = Assert.IsType<User>(createdResult.Value);
        Assert.Equal(newUser.Name, returnedUser.Name);
        Assert.NotEqual(0, returnedUser.Id);
    }

    [Fact]
    public async Task UpdateUser_WithValidData_ReturnsNoContent()
    {
        // Arrange
        var user = await _context.Users.FirstAsync();
        user.DisplayName = "Updated Name";

        // Act
        var result = await _controller.UpdateUser(user.Id, user);

        // Assert
        Assert.IsType<NoContentResult>(result);
        var updatedUser = await _context.Users.FindAsync(user.Id);
        Assert.Equal("Updated Name", updatedUser!.DisplayName);
    }

    [Fact]
    public async Task DeleteUser_WithValidId_ReturnsNoContent()
    {
        // Arrange
        var user = await _context.Users.FirstAsync();

        // Act
        var result = await _controller.DeleteUser(user.Id);

        // Assert
        Assert.IsType<NoContentResult>(result);
        var deletedUser = await _context.Users.FindAsync(user.Id);
        Assert.True(deletedUser!.IsDeleted);
    }
} 