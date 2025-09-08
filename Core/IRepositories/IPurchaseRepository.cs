using Core.Models;

namespace Core.IRepositories
{
    public interface IPurchaseRepository : IRepository<Purchase>
    {
        Task<Purchase> GetByIdWithItemsAsync(int id);
        Task AddPurchaseAsync(Purchase purchase, IEnumerable<PurchaseItem> items);
    }
}
