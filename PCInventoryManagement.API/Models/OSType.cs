using System;
using System.ComponentModel.DataAnnotations;

namespace PCInventoryManagement.API.Models
{
    public class OSType
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        public bool IsDeleted { get; set; }
    }
} 