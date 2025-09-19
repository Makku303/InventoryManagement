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
        [MaxLength(100)]
        public string SKU { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        public int ReorderLevel { get; set; } = 0; // quantity at which to reorder

        public int QuantityOnHand { get; set; } = 0;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Foreign Key to Category

        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        // Concurrency token to ensure data integrity and consistency
        // when multiple users or processes attempt to update the same data simultaneously
        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        public ICollection<PurchaseItem> PurchaseItems { get; set; }
        public ICollection<SaleItem> SaleItems { get; set; }
        public ICollection<InventoryTransaction> InventoryTransactions { get; set; }
    }
}
