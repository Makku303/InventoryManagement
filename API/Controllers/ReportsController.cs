using Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportingService _reportService;

        public ReportsController(IReportingService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("purchases")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetPurchaseReport([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var report = await _reportService.GetPurchasesByDateRangeAsync(startDate.Value, endDate.Value);
            return Ok(report);
        }

        [HttpGet("sales")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetSalesReport([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var report = await _reportService.GetSalesByDateRangeAsync(startDate.Value, endDate.Value);
            return Ok(report);
        }

        [HttpGet("audit-logs")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAuditLogs([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var logs = await _reportService.GetTransactionsByDateRangeAsync(startDate.Value, endDate.Value);
            return Ok(logs);
        }
    }
}
