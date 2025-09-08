using Core.Models;

namespace Core.IRepositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product> GetBySKUAsync(string sku);
        Task<IEnumerable<Product>> GetLowStockAsync(int threshold);
        Task<IEnumerable<Product>> GetOutOfStockAsync();
        Task<IEnumerable<Product>> GetBestSellingAsync(DateTime from, DateTime to, int topN);
    }
}
