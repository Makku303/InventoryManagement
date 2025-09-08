using Core.Models;
using Core.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class InventoryTransactionRepository : Repository<InventoryTransaction>, IInventoryTransactionRepository
    {
        public InventoryTransactionRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<InventoryTransaction>> GetByProductAsync(int productId)
        {
            return await _dbSet.AsNoTracking()
                .Where(it => it.ProductId == productId)
                .OrderByDescending(it => it.PerformedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<InventoryTransaction>> GetByDateRangeAsync(DateTime from, DateTime to)
        {
            return await _dbSet.AsNoTracking()
                .Where(it => it.PerformedAt >= from && it.PerformedAt <= to)
                .OrderByDescending(it => it.PerformedAt)
                .ToListAsync();
        }
    }
}
