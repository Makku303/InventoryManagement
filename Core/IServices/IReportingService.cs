using Core.Models;

namespace Core.IServices
{
    public interface IReportingService
    {
        Task<IEnumerable<InventoryTransaction>> GetTransactionsByDateRangeAsync(DateTime from, DateTime to);
        Task<IEnumerable<Purchase>> GetPurchasesByDateRangeAsync(DateTime from, DateTime to);
        Task<IEnumerable<Sale>> GetSalesByDateRangeAsync(DateTime from, DateTime to);
    }
}
