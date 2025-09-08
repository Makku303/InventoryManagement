using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<Product> Products { get; set; }
    }
}
