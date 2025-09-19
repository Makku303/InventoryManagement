namespace Core.IRepositories
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }
        IPurchaseRepository Purchases { get; }
        ISaleRepository Sales { get; }
        IInventoryTransactionRepository InventoryTransactions { get; }
        ISupplierRepository Suppliers { get; }
        Task<int> SaveChangesAsync();
    }
}
