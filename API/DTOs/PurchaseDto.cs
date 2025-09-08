namespace API.DTOs
{
    public class PurchaseDto
    {
        public int ProductId { get; set; }
        public int SupplierId { get; set; }
        public int Quantity { get; set; }
        public decimal PricePerUnit { get; set; }
    }
}
