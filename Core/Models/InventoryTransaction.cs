using Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class InventoryTransaction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        // positive for incoming, negative for outgoing
        public int ChangeQuantity { get; set; }
        public int PreviousQuantity { get; set; }
        public int NewQuantity { get; set; }

        public TransactionType TransactionType { get; set; }

        // optional reference to Purchase.Id or Sale.Id or manual note
        public int? ReferenceId { get; set; }

        public DateTime PerformedAt { get; set; } = DateTime.UtcNow;

        [MaxLength(1000)]
        public string Notes { get; set; }

        // Foreign Key to ApplicationUser who performed the transaction
        public Guid PerformedById { get; set; }
        public ApplicationUser PerformedBy { get; set; }
    }
}
