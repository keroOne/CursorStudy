using System;
using System.ComponentModel.DataAnnotations;

namespace PCInventoryManagement.API.Models
{
    public class PC
    {
        public int Id { get; set; }

        [Required]
        [StringLength(10)]
        public string ManagementNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string ModelName { get; set; } = string.Empty;

        public int OSTypeId { get; set; }
        public virtual OSType? OSType { get; set; }

        public int? UserId { get; set; }
        public virtual User? User { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
} 