using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class Supplier
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string ContactEmail { get; set; }

        [MaxLength(50)]
        public string ContactPhone { get; set; }

        [MaxLength(500)]
        public string Address { get; set; }

        [MaxLength(1000)]
        public string Notes { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<Purchase> Purchases { get; set; }
    }
}
