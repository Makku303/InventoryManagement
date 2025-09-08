using Core.Models;

namespace Core.IServices
{
    public interface IInventoryService
    {
        //Products
        Task<Product> AddProductAsync(Product product);
        Task<Product> UpdateProductAsync(Product product);
        Task<bool> DeleteProductAsync(int productId);
        Task<Product> GetProductByIdAsync(int id);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold);
        Task<IEnumerable<Product>> GetOutOfStockProductsAsync();
        Task<IEnumerable<Product>> GetBestSellingProductsAsync(DateTime from, DateTime to, int topN);
        Task<IEnumerable<Product>> GetBestSellingProductsAsync(int top);

        //Purchase
        Task<Purchase> AddPurchaseAsync(Purchase purchase, IEnumerable<PurchaseItem> items, Guid userId);
        Task<IEnumerable<Purchase>> GetAllPurchasesAsync();
        Task<Purchase> GetPurchaseByIdAsync(int id);
        Task<bool> AddPurchaseAsync(Purchase purchase);
        Task DeletePurchaseAsync(Purchase purchase);

        //Sales

        Task<IEnumerable<Sale>> GetAllSalesAsync();
        Task<Sale> GetSaleByIdAsync(int id);
        Task<Sale> AddSaleAsync(Sale sale, IEnumerable<SaleItem> items, Guid userId);
        
        Task<bool> AddSaleAsync(Sale sale);
        Task DeleteSaleAsync(Sale sale);
    }
}
