using Core.Models;

namespace Core.IRepositories
{
    public interface ISaleRepository : IRepository<Sale>
    {
        Task<Sale> GetByIdWithItemsAsync(int id);
        Task AddSaleAsync(Sale sale, IEnumerable<SaleItem> items);
        Task<IEnumerable<Sale>> GetByDateRangeAsync(DateTime from, DateTime to);
    }
}
