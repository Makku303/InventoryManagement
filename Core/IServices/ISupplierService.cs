using Core.Models;

namespace Core.IServices
{
    public interface ISupplierService
    {
        Task<Supplier> AddSupplierAsync(Supplier supplier, string userId);
        Task<bool> DeleteSupplierAsync(Supplier supplier, string userId);
        Task<IEnumerable<Supplier>> GetAllSuppliersAsync();
        Task<Supplier> GetSupplierByIdAsync(int id);
        Task<bool> UpdateSupplierAsync(Supplier supplier, string userId);
        Task<Supplier> GetSupplierByIdWithItemsAsync(int id);
    }
}
