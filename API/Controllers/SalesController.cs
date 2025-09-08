using API.DTOs;
using Core.IServices;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public SalesController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetAllSales()
        {
            var sales = await _inventoryService.GetAllSalesAsync();
            return Ok(sales);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetSaleById(int id)
        {
            var sale = await _inventoryService.GetSaleByIdAsync(id);
            if (sale == null)
                return NotFound();

            return Ok(sale);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> AddSale([FromBody] SaleDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            List<SaleItem> items = new List<SaleItem>();
            items.Add(new SaleItem() { 
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                UnitPrice = dto.PricePerUnit
            });
            var sale = new Sale
            {
                SaleItems = items,
                SaleDate = System.DateTime.UtcNow
            };

            var result = await _inventoryService.AddSaleAsync(sale);
            if (!result)
                return BadRequest(new { message = "Sale could not be processed (maybe insufficient stock)." });

            return Ok(new { message = "Sale logged successfully" });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteSale(int id)
        {
            var sale = await _inventoryService.GetSaleByIdAsync(id);
            if (sale == null)
                return NotFound();

            await _inventoryService.DeleteSaleAsync(sale);
            return Ok(new { message = "Sale deleted successfully" });
        }
    }
}
