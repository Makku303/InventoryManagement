using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(250)]
        public string Name { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [Required]
        [MaxLength(100)]
        public string SKU { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        public int ReorderLevel { get; set; } = 0;

        public int QuantityOnHand { get; set; } = 0;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Concurrency token
        [Timestamp]
        public byte[] RowVersion { get; set; }

        public ICollection<PurchaseItem> PurchaseItems { get; set; }
        public ICollection<SaleItem> SaleItems { get; set; }
        public ICollection<InventoryTransaction> InventoryTransactions { get; set; }
    }
}
