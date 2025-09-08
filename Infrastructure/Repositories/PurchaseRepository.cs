using Core.Models;
using Core.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
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
    }
}
