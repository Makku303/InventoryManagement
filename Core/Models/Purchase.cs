using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class Purchase
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }

        [MaxLength(200)]
        public string InvoiceNumber { get; set; }

        public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        public Guid CreatedById { get; set; }
        public ApplicationUser CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<PurchaseItem> PurchaseItems { get; set; }
    }
}
