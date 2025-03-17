using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PCInventoryManagement.API.Models
{
    public class User
    {
        public User()
        {
            PCs = new List<PC>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string ADAccount { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string DisplayName { get; set; } = string.Empty;

        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties
        public int LocationId { get; set; }
        public virtual Location? Location { get; set; }
        public virtual ICollection<PC> PCs { get; set; }
    }
} 