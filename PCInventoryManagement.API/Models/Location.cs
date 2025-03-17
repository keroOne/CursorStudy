using System.ComponentModel.DataAnnotations;

namespace PCInventoryManagement.API.Models;

public class Location
{
    public int Id { get; set; }

    [Required]
    [StringLength(10)]
    public string Code { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string Name { get; set; } = string.Empty;

    public bool IsDeleted { get; set; }

    // Navigation Properties
    public ICollection<User> Users { get; set; } = new List<User>();
} 