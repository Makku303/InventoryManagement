using Core.IRepositories;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories
{
    public class SupplierRepository: Repository<Supplier>, ISupplierRepository
    {
        public SupplierRepository(ApplicationDbContext context) : base(context) { }

        public Task<Supplier> GetByIdWithItemsAsync(int id)
        {
            return _dbSet.Include(p=>p.Purchases)
                .ThenInclude(pi=>pi.PurchaseItems)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}
