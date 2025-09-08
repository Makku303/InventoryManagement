namespace Core.IServices
{
    public interface IAuditLogService
    {
        Task ClearAuditLogsAsync(DateTime? beforeDate);
        Task DeleteAuditLogAsync(int id);
        Task<bool> GetAuditLogByIdAsync(int id); //to be updated to the correct model
        Task<bool> GetAuditLogsAsync(DateTime? startDate, DateTime? endDate); //to be updated to the correct model
    }
}
