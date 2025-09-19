using Core.Models;

namespace Core.IServices
{
    public interface IDashboardService
    {
        Task<IEnumerable<Product>> GetBestSellingProductsAsync(DateTime from, DateTime to, int topN);
        Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold); 
        Task<IEnumerable<Product>> GetOutOfStockProductsAsync();
        Task<IEnumerable<Purchase>> GetRecentPurchasesAsync(DateTime from, DateTime to);
        Task<IEnumerable<Sale>> GetRecentSalesAsync(DateTime from, DateTime to);
    }
}
