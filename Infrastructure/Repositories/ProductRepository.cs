using Core.Models;
using Core.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Product> GetBySKUAsync(string sku)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(p => p.SKU == sku);
        }

        public async Task<IEnumerable<Product>> GetLowStockAsync(int threshold)
        {
            return await _dbSet.AsNoTracking().Where(p => p.QuantityOnHand <= threshold && p.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetOutOfStockAsync()
        {
            return await _dbSet.AsNoTracking().Where(p => p.QuantityOnHand == 0 && p.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetBestSellingAsync(DateTime from, DateTime to, int topN)
        {
            return await _context.SaleItems
                .Where(si => si.Sale.SaleDate >= from && si.Sale.SaleDate <= to)
                .GroupBy(si => si.ProductId)
                .Select(g => new { ProductId = g.Key, TotalSold = g.Sum(x => x.Quantity) })
                .OrderByDescending(x => x.TotalSold)
                .Take(topN)
                .Join(_context.Products, g => g.ProductId, p => p.Id, (g, p) => p)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
