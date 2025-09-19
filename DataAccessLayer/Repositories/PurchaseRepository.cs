using Core.IRepositories;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories
{
    public class PurchaseRepository : Repository<Purchase>, IPurchaseRepository
    {
        public PurchaseRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Purchase> GetByIdWithItemsAsync(int id)
        {
            return await _dbSet.Include(p => p.PurchaseItems)
                               .ThenInclude(pi => pi.Product)
                               .AsNoTracking()
                               .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddPurchaseAsync(Purchase purchase, IEnumerable<PurchaseItem> items)
        {
            await _dbSet.AddAsync(purchase);
            await _context.PurchaseItems.AddRangeAsync(items);
        }

        public async Task<IEnumerable<Purchase>> GetByDateRangeAsync(DateTime from, DateTime to)
        {
            return await _dbSet.AsNoTracking()
                .Where(it => it.CreatedAt >= from && it.CreatedAt <= to)
                .OrderByDescending(it => it.CreatedAt)
                .ToListAsync();
        }
    }
}
