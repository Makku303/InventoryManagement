using Core.Models;

namespace Core.IServices
{
    public interface ISupplierService
    {
        Task<Supplier> AddSupplierAsync(Supplier supplier);
        Task<bool> DeleteSupplierAsync(Supplier supplier);
        Task<IEnumerable<Supplier>> GetAllSuppliersAsync();
        Task<Supplier> GetSupplierByIdAsync(int id);
        Task<bool> UpdateSupplierAsync(Supplier supplier);
    }
}
