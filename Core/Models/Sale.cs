using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class Sale
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(200)]
        public string CustomerName { get; set; }
        [MaxLength(200)]
        public string InvoiceNumber { get; set; }
        public DateTime SaleDate { get; set; } = DateTime.UtcNow;
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Foreign Key to ApplicationUser who created the sale
        public Guid CreatedById { get; set; }
        public ApplicationUser CreatedBy { get; set; }

        // Navigation property for related sale items
        public ICollection<SaleItem> SaleItems { get; set; }
    }
}
