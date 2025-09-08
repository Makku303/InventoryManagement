using API.DTOs;
using Core.IServices;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SuppliersController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetAllSuppliers()
        {
            var suppliers = await _supplierService.GetAllSuppliersAsync();
            return Ok(suppliers);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetSupplierById(int id)
        {
            var supplier = await _supplierService.GetSupplierByIdAsync(id);
            if (supplier == null)
                return NotFound();

            return Ok(supplier);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> AddSupplier([FromBody] SupplierDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var supplier = new Supplier
            {
                Name = dto.Name,
                ContactPhone = dto.ContactInfo
            };

            await _supplierService.AddSupplierAsync(supplier);
            return Ok(new { message = "Supplier added successfully" });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> UpdateSupplier(int id, [FromBody] SupplierDto dto)
        {
            var supplier = await _supplierService.GetSupplierByIdAsync(id);
            if (supplier == null)
                return NotFound();

            supplier.Name = dto.Name;
            supplier.ContactPhone = dto.ContactInfo;

            await _supplierService.UpdateSupplierAsync(supplier);
            return Ok(new { message = "Supplier updated successfully" });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            var supplier = await _supplierService.GetSupplierByIdAsync(id);
            if (supplier == null)
                return NotFound();

            await _supplierService.DeleteSupplierAsync(supplier);
            return Ok(new { message = "Supplier deleted successfully" });
        }
    }
}
