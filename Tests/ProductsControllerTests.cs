using Infrastructure.Models;
using InventorySystem.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class ProductsControllerTests
    {
        private readonly Mock<IProductService> _productServiceMock;
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
            _productServiceMock = new Mock<IProductService>();
            _controller = new ProductsController(_productServiceMock.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOk_WithListOfProducts()
        {
            // Arrange
            var products = new List<Product> { new Product { Id = 1, Name = "Product A", Quantity = 10 } };
            _productServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(products);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnProducts = Assert.IsType<List<Product>>(okResult.Value);
            Assert.Single(returnProducts);
        }

        [Fact]
        public async Task GetById_ShouldReturnOk_WhenProductExists()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product A" };
            _productServiceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(product);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnProduct = Assert.IsType<Product>(okResult.Value);
            Assert.Equal(1, returnProduct.Id);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            _productServiceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((Product)null);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_ShouldReturnCreatedAtAction_WhenSuccessful()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "New Product" };
            _productServiceMock.Setup(s => s.CreateAsync(product)).ReturnsAsync(product);

            // Act
            var result = await _controller.Create(product);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnProduct = Assert.IsType<Product>(createdResult.Value);
            Assert.Equal("New Product", returnProduct.Name);
        }

        [Fact]
        public async Task Update_ShouldReturnNoContent_WhenSuccessful()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Updated Product" };
            _productServiceMock.Setup(s => s.UpdateAsync(1, product)).ReturnsAsync(true);

            // Act
            var result = await _controller.Update(1, product);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Update_ShouldReturnNotFound_WhenProductNotExist()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Updated Product" };
            _productServiceMock.Setup(s => s.UpdateAsync(1, product)).ReturnsAsync(false);

            // Act
            var result = await _controller.Update(1, product);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ShouldReturnNoContent_WhenSuccessful()
        {
            // Arrange
            _productServiceMock.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenProductNotExist()
        {
            // Arrange
            _productServiceMock.Setup(s => s.DeleteAsync(1)).ReturnsAsync(false);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
