using API.DTOs;
using Core.IServices;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public ProductsController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _inventoryService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _inventoryService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> AddProduct([FromBody] ProductDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = new Product
            {
                Name = dto.Name,
                //Category = dto.Category,
                SKU = dto.SKU,
                QuantityOnHand = dto.Quantity,
                UnitPrice = dto.Price
            };

            await _inventoryService.AddProductAsync(product);
            return Ok(new { message = "Product added successfully" });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = await _inventoryService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();

            product.Name = dto.Name;
            //product.Category = dto.Category;
            product.SKU = dto.SKU;
            product.QuantityOnHand = dto.Quantity;
            product.UnitPrice = dto.Price;

            await _inventoryService.UpdateProductAsync(product);
            return Ok(new { message = "Product updated successfully" });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _inventoryService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();

            await _inventoryService.DeleteProductAsync(product.Id);
            return Ok(new { message = "Product deleted successfully" });
        }

        // Panels
        [HttpGet("low-stock")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetLowStockProducts([FromQuery] int threshold = 10)
        {
            var products = await _inventoryService.GetLowStockProductsAsync(threshold);
            return Ok(products);
        }

        [HttpGet("out-of-stock")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetOutOfStockProducts()
        {
            var products = await _inventoryService.GetOutOfStockProductsAsync();
            return Ok(products);
        }

        [HttpGet("best-selling")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetBestSellingProducts([FromQuery] int top = 5)
        {
            var products = await _inventoryService.GetBestSellingProductsAsync(top);
            return Ok(products);
        }
    }
}
