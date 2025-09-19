using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class SaleItem
    {
        [Key]
        public int Id { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal LineTotal { get; set; }

        // Foreign Key to Product
        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        // Foreign Key to Sale
        [Required]
        public int SaleId { get; set; }
        public Sale Sale { get; set; }
    }
}
