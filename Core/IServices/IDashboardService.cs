using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IServices
{
    public interface IDashboardService
    {
        Task<Product> GetBestSellingProductsAsync(); //To be updated if needed a list results
        Task<Product> GetLowStockProductsAsync(); //To be updated if needed a list results
        Task<Product> GetOutOfStockProductsAsync(); //To be updated if needed a list results
        Task<Product> GetRecentPurchasesAsync(); //To be updated if needed a list results
        Task<Product> GetRecentSalesAsync(); //To be updated if needed a list results
    }
}
