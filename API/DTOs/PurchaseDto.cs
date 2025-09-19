using Core.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Dtos
{
    public class PurchaseDto
    {
        public string InvoiceNumber { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int SupplierId { get; set; }


        // Navigation property for related purchase items
        public ICollection<PurchaseItemDto> PurchaseItems { get; set; }
    }
}
