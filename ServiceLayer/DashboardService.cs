using Core.IRepositories;
using Core.IServices;
using Core.Models;

namespace ServiceLayer
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;
        public DashboardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Task<IEnumerable<Product>> GetBestSellingProductsAsync(DateTime from, DateTime to, int topN)
        {
            if (from > to)
                throw new ArgumentException("The 'from' date must be earlier than or equal to the 'to' date.");
            if (to > DateTime.Now)
                throw new ArgumentException("The 'to' date cannot be in the future.");
            var products = _unitOfWork.Products.GetBestSellingAsync(from, to, topN).Result;
            return Task.FromResult(products);

        }

        public Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold)
        {
            var products = _unitOfWork.Products.GetLowStockAsync(threshold).Result;
            return Task.FromResult(products);
        }

        public Task<IEnumerable<Product>> GetOutOfStockProductsAsync()
        {
            var products = _unitOfWork.Products.GetOutOfStockAsync().Result;
            return Task.FromResult(products);
        }

        public Task<IEnumerable<Purchase>> GetRecentPurchasesAsync(DateTime from, DateTime to)
        {
            if (from > to)
                throw new ArgumentException("The 'from' date must be earlier than or equal to the 'to' date.");
            if (to > DateTime.UtcNow)
                throw new ArgumentException("The 'to' date cannot be in the future.");
            var purchases = _unitOfWork.Purchases.GetByDateRangeAsync(from, to).Result;
            return Task.FromResult(purchases);
        }

        public Task<IEnumerable<Sale>> GetRecentSalesAsync(DateTime from, DateTime to)
        {
            if (from > to)
                throw new ArgumentException("The 'from' date must be earlier than or equal to the 'to' date.");
            if (to > DateTime.Now)
                throw new ArgumentException("The 'to' date cannot be in the future.");
            var sales = _unitOfWork.Sales.GetByDateRangeAsync(from, to).Result;
            return Task.FromResult(sales);
        }
    }
}
