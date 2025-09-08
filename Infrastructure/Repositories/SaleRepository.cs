using Core.Models;
using Core.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
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
    }
}
