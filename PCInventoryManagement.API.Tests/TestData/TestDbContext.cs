using Microsoft.EntityFrameworkCore;
using PCInventoryManagement.API.Data;
using PCInventoryManagement.API.Models;
using System;

namespace PCInventoryManagement.API.Tests.TestData
{
    public class TestDbContext : ApplicationDbContext
    {
        public TestDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
    }
} 