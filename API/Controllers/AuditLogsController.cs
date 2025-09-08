using Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AuditLogsController : ControllerBase
    {
        private readonly IAuditLogService _auditLogService;

        public AuditLogsController(IAuditLogService auditLogService)
        {
            _auditLogService = auditLogService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAuditLogs([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var logs = await _auditLogService.GetAuditLogsAsync(startDate, endDate);
            return Ok(logs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuditLogById(int id)
        {
            var log = await _auditLogService.GetAuditLogByIdAsync(id);
            if (log == null)
                return NotFound();

            return Ok(log);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuditLog(int id)
        {
            var log = await _auditLogService.GetAuditLogByIdAsync(id);
            if (log == null)
                return NotFound();

            await _auditLogService.DeleteAuditLogAsync(id);
            return Ok(new { message = "Audit log deleted successfully" });
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> ClearAuditLogs([FromQuery] DateTime? beforeDate)
        {
            await _auditLogService.ClearAuditLogsAsync(beforeDate);
            return Ok(new { message = "Audit logs cleared successfully" });
        }
    }
}
