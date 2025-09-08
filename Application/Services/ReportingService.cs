using Core.IRepositories;
using Core.IServices;
using Core.Models;

namespace Application.Services
{
    public class ReportingService : IReportingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReportingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<InventoryTransaction>> GetTransactionsByDateRangeAsync(DateTime from, DateTime to)
        {
            return await _unitOfWork.InventoryTransactions.GetByDateRangeAsync(from, to);
        }

        public async Task<IEnumerable<Purchase>> GetPurchasesByDateRangeAsync(DateTime from, DateTime to)
        {
            return await _unitOfWork.Purchases.ListAsync();
        }

        public async Task<IEnumerable<Sale>> GetSalesByDateRangeAsync(DateTime from, DateTime to)
        {
            return await _unitOfWork.Sales.ListAsync();
        }
    }
}
