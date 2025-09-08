using Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Staff")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("low-stock")]
        public async Task<IActionResult> GetLowStockProducts()
        {
            var products = await _dashboardService.GetLowStockProductsAsync();
            return Ok(products);
        }

        [HttpGet("out-of-stock")]
        public async Task<IActionResult> GetOutOfStockProducts()
        {
            var products = await _dashboardService.GetOutOfStockProductsAsync();
            return Ok(products);
        }

        [HttpGet("best-sellers")]
        public async Task<IActionResult> GetBestSellingProducts()
        {
            var products = await _dashboardService.GetBestSellingProductsAsync();
            return Ok(products);
        }

        [HttpGet("recent-sales")]
        public async Task<IActionResult> GetRecentSales()
        {
            var sales = await _dashboardService.GetRecentSalesAsync();
            return Ok(sales);
        }

        [HttpGet("recent-purchases")]
        public async Task<IActionResult> GetRecentPurchases()
        {
            var purchases = await _dashboardService.GetRecentPurchasesAsync();
            return Ok(purchases);
        }
    }
}
