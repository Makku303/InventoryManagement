using Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Roles = "Admin,Staff")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("low-stock")]
        public async Task<IActionResult> GetLowStockProducts([FromQuery] int threshold = 10)
        {
            var products = await _dashboardService.GetLowStockProductsAsync(threshold);
            return Ok(products);
        }

        [HttpGet("out-of-stock")]
        public async Task<IActionResult> GetOutOfStockProducts()
        {
            var products = await _dashboardService.GetOutOfStockProductsAsync();
            return Ok(products);
        }

        [HttpGet("best-sellers")]
        public async Task<IActionResult> GetBestSellingProducts([FromQuery] DateTime from, [FromQuery] DateTime to, [FromQuery] int top)
        {
            var products = await _dashboardService.GetBestSellingProductsAsync(from, to, top);
            return Ok(products);
        }

        [HttpGet("recent-sales")]
        public async Task<IActionResult> GetRecentSales(DateTime from, DateTime to)
        {
            var sales = await _dashboardService.GetRecentSalesAsync(from, to);
            return Ok(sales);
        }

        [HttpGet("recent-purchases")]
        public async Task<IActionResult> GetRecentPurchases(DateTime from, DateTime to)
        {
            var purchases = await _dashboardService.GetRecentPurchasesAsync(from, to);
            return Ok(purchases);
        }
    }
}
