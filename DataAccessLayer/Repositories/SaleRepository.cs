using Core.IRepositories;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories
{
    public class SaleRepository : Repository<Sale>, ISaleRepository
    {
        public SaleRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Sale> GetByIdWithItemsAsync(int id)
        {
            return await _dbSet.Include(s => s.SaleItems)
                               .ThenInclude(si => si.Product)
                               .AsNoTracking()
                               .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task AddSaleAsync(Sale sale, IEnumerable<SaleItem> items)
        {
            await _dbSet.AddAsync(sale);
            await _context.SaleItems.AddRangeAsync(items);
        }

        public async Task<IEnumerable<Sale>> GetByDateRangeAsync(DateTime from, DateTime to)
        {
            return await _dbSet.AsNoTracking()
                .Where(it => it.CreatedAt >= from && it.CreatedAt <= to)
                .OrderByDescending(it => it.CreatedAt)
                .ToListAsync();
        }
    }
}
