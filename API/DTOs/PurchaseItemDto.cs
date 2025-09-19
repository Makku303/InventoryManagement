namespace API.Dtos
{
    public class PurchaseItemDto
    {
        public int Quantity { get; set; }
        public decimal LineTotal { get; set; }
        public int PurchaseId { get; set; }
        public int ProductId { get; set; }
    }
}
