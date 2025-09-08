using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        [Required]
        [MaxLength(200)]
        public string FullName { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
