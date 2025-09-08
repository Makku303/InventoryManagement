using API.Controllers;
using Core.IServices;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Tests
{
    public class SuppliersControllerTests
    {
        private readonly Mock<ISupplierService> _supplierServiceMock;
        private readonly SuppliersController _controller;

        public SuppliersControllerTests()
        {
            _supplierServiceMock = new Mock<ISupplierService>();
            _controller = new SuppliersController(_supplierServiceMock.Object);
        }

        [Fact]
        public async Task GetSuppliers_ShouldReturnOk_WithSupplierList()
        {
            // Arrange
            var suppliers = new List<Supplier> { new Supplier { Id = 1, Name = "Test Supplier" } };
            _supplierServiceMock.Setup(s => s.GetAllSuppliersAsync()).ReturnsAsync(suppliers);

            // Act
            var result = await _controller.GetAllSuppliers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnData = Assert.IsType<List<Supplier>>(okResult.Value);
            Assert.Single(returnData);
        }

        [Fact]
        public async Task GetSupplier_ShouldReturnOk_WhenFound()
        {
            // Arrange
            var supplier = new Supplier { Id = 1, Name = "Supplier A" };
            _supplierServiceMock.Setup(s => s.GetSupplierByIdAsync(1)).ReturnsAsync(supplier);

            // Act
            var result = await _controller.GetSupplierById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnData = Assert.IsType<Supplier>(okResult.Value);
            Assert.Equal("Supplier A", returnData.Name);
        }

        [Fact]
        public async Task GetSupplier_ShouldReturnNotFound_WhenMissing()
        {
            // Arrange
            _supplierServiceMock.Setup(s => s.GetSupplierByIdAsync(1)).ReturnsAsync((Supplier?)null);

            // Act
            var result = await _controller.GetSupplierById(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CreateSupplier_ShouldReturnCreatedAtAction()
        {
            // Arrange
            var supplier = new Supplier { Id = 1, Name = "New Supplier" };
            _supplierServiceMock.Setup(s => s.AddSupplierAsync(supplier)).ReturnsAsync(supplier);

            // Act
            var result = await _controller.AddSupplier(new API.DTOs.SupplierDto() { 
                Name = supplier.Name
            });

            // Assert
            var createdAt = Assert.IsType<CreatedAtActionResult>(result);
            var returnData = Assert.IsType<Supplier>(createdAt.Value);
            Assert.Equal("New Supplier", returnData.Name);
        }

        [Fact]
        public async Task UpdateSupplier_ShouldReturnNoContent_WhenSuccessful()
        {
            // Arrange
            var supplier = new Supplier { Id = 1, Name = "Updated Supplier" };
            _supplierServiceMock.Setup(s => s.UpdateSupplierAsync(supplier)).ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateSupplier(1, new API.DTOs.SupplierDto() { 
                Name = supplier.Name
            });

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateSupplier_ShouldReturnNotFound_WhenMissing()
        {
            // Arrange
            var supplier = new Supplier { Id = 1, Name = "Updated Supplier" };
            _supplierServiceMock.Setup(s => s.UpdateSupplierAsync(supplier)).ReturnsAsync(false);

            // Act
            var result = await _controller.UpdateSupplier(1, new API.DTOs.SupplierDto() { Name = supplier.Name});

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteSupplier_ShouldReturnNoContent_WhenSuccessful()
        {
            // Arrange
            var supplier = new Supplier { Id = 1, Name = "Deleted Supplier" };
            _supplierServiceMock.Setup(s => s.DeleteSupplierAsync(supplier)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteSupplier(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteSupplier_ShouldReturnNotFound_WhenMissing()
        {
            // Arrange
            var supplier = new Supplier { Id = 1, Name = "Deleted Supplier" };
            _supplierServiceMock.Setup(s => s.DeleteSupplierAsync(supplier)).ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteSupplier(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
