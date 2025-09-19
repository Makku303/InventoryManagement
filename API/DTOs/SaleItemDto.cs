namespace API.Dtos
{
    public class SaleItemDto
    {
        public int Quantity { get; set; }
        public decimal LineTotal { get; set; }
        public int ProductId { get; set; }
        public int SaleId { get; set; }
    }
}
