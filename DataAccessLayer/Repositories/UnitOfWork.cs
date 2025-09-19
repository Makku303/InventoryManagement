using Core.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public readonly ApplicationDbContext _context;

        public IProductRepository Products { get; }
        public IPurchaseRepository Purchases { get; }
        public ISaleRepository Sales { get; }
        public IInventoryTransactionRepository InventoryTransactions { get; }
        public ISupplierRepository Suppliers { get; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Products = new ProductRepository(_context);
            Purchases = new PurchaseRepository(_context);
            Sales = new SaleRepository(_context);
            InventoryTransactions = new InventoryTransactionRepository(_context);
            Suppliers = new SupplierRepository(_context);
            
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
