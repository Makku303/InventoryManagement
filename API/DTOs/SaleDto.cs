namespace API.Dtos
{
    public class SaleDto
    {
        public string CustomerName { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime SaleDate { get; set; } = DateTime.UtcNow;
        public decimal TotalAmount { get; set; }

        // Navigation property for related sale items
        public ICollection<SaleItemDto> SaleItems { get; set; }
    }
}
