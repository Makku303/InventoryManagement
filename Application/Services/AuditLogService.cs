using Core.IServices;

namespace Application.Services
{
    public class AuditLogService : IAuditLogService
    {
        public Task ClearAuditLogsAsync(DateTime? beforeDate)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAuditLogAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetAuditLogByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetAuditLogsAsync(DateTime? startDate, DateTime? endDate)
        {
            throw new NotImplementedException();
        }
    }
}
