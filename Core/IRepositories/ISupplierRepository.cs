using Core.Models;

namespace Core.IRepositories
{
    public interface ISupplierRepository : IRepository<Supplier>
    {
        Task<Supplier> GetByIdWithItemsAsync(int id);
    }
}
