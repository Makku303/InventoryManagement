using Core.IRepositories;
using Core.IServices;
using Core.Models;

namespace ServiceLayer
{
    public class ReportingService: IReportingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReportingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<InventoryTransaction>> GetTransactionsByDateRangeAsync(DateTime from, DateTime to)
        {
            if (from > to)
                throw new ArgumentException("The 'from' date must be earlier than or equal to the 'to' date.");
            
            if (to > DateTime.Now)
                throw new ArgumentException("The 'to' date cannot be in the future.");
            
            return await _unitOfWork.InventoryTransactions.GetByDateRangeAsync(from, to);
        }

        public async Task<IEnumerable<Purchase>> GetPurchasesByDateRangeAsync(DateTime from, DateTime to)
        {
            if (from > to)
                throw new ArgumentException("The 'from' date must be earlier than or equal to the 'to' date.");

            if (to > DateTime.Now)
                throw new ArgumentException("The 'to' date cannot be in the future.");

            return await _unitOfWork.Purchases.GetByDateRangeAsync(from, to);
        }

        public async Task<IEnumerable<Sale>> GetSalesByDateRangeAsync(DateTime from, DateTime to)
        {
            if (from > to)
                throw new ArgumentException("The 'from' date must be earlier than or equal to the 'to' date.");

            if (to > DateTime.Now)
                throw new ArgumentException("The 'to' date cannot be in the future.");

            return await _unitOfWork.Sales.GetByDateRangeAsync(from, to);
        }
    }
}
