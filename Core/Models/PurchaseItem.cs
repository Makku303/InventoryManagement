using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class PurchaseItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PurchaseId { get; set; }
        public Purchase Purchase { get; set; }

        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitCost { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal LineTotal { get; set; }
    }
}
