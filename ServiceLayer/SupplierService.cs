using Core.IRepositories;
using Core.IServices;
using Core.Models;

namespace ServiceLayer
{
    public class SupplierService : ISupplierService
    {
        private readonly IUnitOfWork _unitOfWork;
        public SupplierService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Supplier> AddSupplierAsync(Supplier supplier, string userId)
        {
            if (supplier == null) {
                throw new ArgumentNullException(nameof(supplier));
            }

            _unitOfWork.Suppliers.AddAsync(supplier);
            await _unitOfWork.SaveChangesAsync();
            return supplier;
        }

        public Task<bool> DeleteSupplierAsync(Supplier supplier, string userId)
        {
            if (supplier == null) {
                throw new ArgumentNullException(nameof(supplier));
            }

            var id = supplier.Id;
            var existingSupplier = _unitOfWork.Suppliers.GetByIdAsync(id);
            if (existingSupplier == null) {
                throw new KeyNotFoundException($"Supplier with ID {id} not found.");
            }

            _unitOfWork.Suppliers.Delete(supplier);
            return _unitOfWork.SaveChangesAsync().ContinueWith(t => t.Result > 0);
        }

        public Task<IEnumerable<Supplier>> GetAllSuppliersAsync()
        {
            return _unitOfWork.Suppliers.ListAsync();
        }

        public Task<Supplier> GetSupplierByIdAsync(int id)
        {
            return _unitOfWork.Suppliers.GetByIdAsync(id);
        }

        public Task<Supplier> GetSupplierByIdWithItemsAsync(int id)
        {
            return _unitOfWork.Suppliers.GetByIdWithItemsAsync(id);
        }

        public Task<bool> UpdateSupplierAsync(Supplier supplier, string userId)
        {
            if (supplier == null) {
                throw new ArgumentNullException(nameof(supplier));
            }
            var id = supplier.Id;
            var existingSupplier = _unitOfWork.Suppliers.GetByIdAsync(id);
            if (existingSupplier == null) {
                throw new KeyNotFoundException($"Supplier with ID {id} not found.");
            }

            _unitOfWork.Suppliers.Update(supplier);
            return _unitOfWork.SaveChangesAsync().ContinueWith(t => t.Result > 0);
        }
    }
}
