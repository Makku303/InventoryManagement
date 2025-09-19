using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IServices
{
    public interface IInventoryService
    {
        //Products
        Task<Product> AddProductAsync(Product product, string userId);
        Task<Product> UpdateProductAsync(Product product, string userId);
        Task<bool> DeleteProductAsync(int productId, string userId);
        Task<Product> GetProductByIdAsync(int id);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductBySKUAsync(string sku);
        Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold);
        Task<IEnumerable<Product>> GetOutOfStockProductsAsync();
        Task<IEnumerable<Product>> GetBestSellingProductsAsync(DateTime from, DateTime to, int topN);

        //Purchase        
        Task<Purchase> AddPurchaseAsync(Purchase purchase, string userId);
        Task<Purchase> UpdatePurchaseAsync(Purchase purchase, string userId);
        Task<bool> DeletePurchaseAsync(int purchaseId, string userId);
        Task<IEnumerable<Purchase>> GetAllPurchasesAsync();
        Task<Purchase> GetPurchaseByIdAsync(int id);
        Task<Purchase> GetPurchaseByIdWithItemsAsync(int id);
        Task<Purchase> AddPurchaseAsync(Purchase purchase, IEnumerable<PurchaseItem> items, Guid userId);
        Task<IEnumerable<Purchase>> GetPurchasesByDateRangeAsync(DateTime from, DateTime to);

        //Sales
        Task<Sale> AddSaleAsync(Sale sale, string userId);
        Task<Sale> UpdatePurchaseAsync(Sale sale, string userId);
        Task<bool> DeleteSaleAsync(int saleId, string userId);
        Task<IEnumerable<Sale>> GetAllSalesAsync();
        Task<Sale> GetSaleByIdAsync(int id);
        Task<Sale> GetSaleByIdWithItemsAsync(int id);
        Task<Sale> AddSaleAsync(Sale sale, IEnumerable<SaleItem> items, Guid userId);
        Task<IEnumerable<Sale>> GetSalesByDateRangeAsync(DateTime from, DateTime to);
    }
}
