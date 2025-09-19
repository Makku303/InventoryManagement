using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        [Required]
        [MaxLength(200)]
        public string FirstName { get; set; }

        [Required]
        public string MiddleInitial { get; set; }

        [Required]
        [MaxLength(200)]
        public string LastName { get; set; }


        [NotMapped] // Prevents EF from mapping this to the database
        public string FullName => $"{FirstName} {MiddleInitial}. {LastName}";

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
