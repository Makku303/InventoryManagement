using Core.IServices;
using Core.Models;

namespace Application.Services
{
    public class DashboardService : IDashboardService
    {
        public Task<Product> GetBestSellingProductsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetLowStockProductsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetOutOfStockProductsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetRecentPurchasesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetRecentSalesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
