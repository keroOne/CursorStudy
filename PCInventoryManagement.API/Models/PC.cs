using System;
using System.ComponentModel.DataAnnotations;

namespace PCInventoryManagement.API.Models
{
    public class PC
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string ManagementNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string ModelName { get; set; } = string.Empty;

        public int OSTypeId { get; set; }
        public virtual OSType? OSType { get; set; }

        public int? CurrentUserId { get; set; }
        public virtual User? CurrentUser { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
} 