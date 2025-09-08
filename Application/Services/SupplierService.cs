using Core.IServices;
using Core.Models;

namespace Application.Services
{
    public class SupplierService : ISupplierService
    {
        public Task<Supplier> AddSupplierAsync(Supplier supplier)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteSupplierAsync(Supplier supplier)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Supplier>> GetAllSuppliersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Supplier> GetSupplierByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateSupplierAsync(Supplier supplier)
        {
            throw new NotImplementedException();
        }
    }
}
