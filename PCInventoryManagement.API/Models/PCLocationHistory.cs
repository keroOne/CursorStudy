using System;
using System.ComponentModel.DataAnnotations;

namespace PCInventoryManagement.API.Models
{
    public class PCLocationHistory
    {
        public int Id { get; set; }

        public int PCId { get; set; }
        public PC PC { get; set; } = null!;

        public int? FromUserId { get; set; }
        public User? FromUser { get; set; }

        public int? ToUserId { get; set; }
        public User? ToUser { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
} 