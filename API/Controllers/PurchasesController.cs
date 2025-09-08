using API.DTOs;
using Core.IServices;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PurchasesController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public PurchasesController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetAllPurchases()
        {
            var purchases = await _inventoryService.GetAllPurchasesAsync();
            return Ok(purchases);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetPurchaseById(int id)
        {
            var purchase = await _inventoryService.GetPurchaseByIdAsync(id);
            if (purchase == null)
                return NotFound();

            return Ok(purchase);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> AddPurchase([FromBody] PurchaseDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            List<PurchaseItem> items =  new List<PurchaseItem>();

            items.Add(new PurchaseItem() { 
             ProductId = dto.ProductId,
             Quantity = dto.Quantity,
             UnitCost = dto.PricePerUnit
            });
            var purchase = new Purchase
            {
                SupplierId = dto.SupplierId,
                PurchaseItems = items,
                PurchaseDate = System.DateTime.UtcNow
            };

            var result = await _inventoryService.AddPurchaseAsync(purchase);
            if (!result)
                return BadRequest(new { message = "Purchase could not be processed." });

            return Ok(new { message = "Purchase logged successfully" });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePurchase(int id)
        {
            var purchase = await _inventoryService.GetPurchaseByIdAsync(id);
            if (purchase == null)
                return NotFound();

            await _inventoryService.DeletePurchaseAsync(purchase);
            return Ok(new { message = "Purchase deleted successfully" });
        }
    }
}
