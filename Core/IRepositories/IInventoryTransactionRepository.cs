using Core.Models;

namespace Core.IRepositories
{
    public interface IInventoryTransactionRepository : IRepository<InventoryTransaction>
    {
        Task<IEnumerable<InventoryTransaction>> GetByProductAsync(int productId);
        Task<IEnumerable<InventoryTransaction>> GetByDateRangeAsync(DateTime from, DateTime to);
    }
}
