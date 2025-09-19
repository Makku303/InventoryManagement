using API.Dtos;
using Core.IServices;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
            List<PurchaseItem> items = new List<PurchaseItem>();
            var purchaseItems = dto.PurchaseItems;
            foreach (var item in purchaseItems)
            {
                items.Add(new PurchaseItem()
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    LineTotal = item.LineTotal
                });
            }
            var purchase = new Purchase
            {
                SupplierId = dto.SupplierId,
                PurchaseItems = items,
                PurchaseDate = System.DateTime.UtcNow
            };

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "system";
            var result = await _inventoryService.AddPurchaseAsync(purchase, items, new Guid(userId));

            if (result.GetType() != typeof(Purchase))
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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "system";
            await _inventoryService.DeletePurchaseAsync(purchase.Id, userId);
            return Ok(new { message = "Purchase deleted successfully" });
        }
    }
}
