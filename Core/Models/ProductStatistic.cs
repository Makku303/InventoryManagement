using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class ProductStatistic
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public long TotalSold { get; set; }
        public DateTime? LastSoldAt { get; set; }
    }
}
