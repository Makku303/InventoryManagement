using API.Controllers;
using Core.IServices;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Tests
{
    public class SalesControllerTests
    {
        private readonly Mock<IInventoryService> _saleServiceMock;
        private readonly SalesController _controller;

        public SalesControllerTests()
        {
            _saleServiceMock = new Mock<IInventoryService>();
            _controller = new SalesController(_saleServiceMock.Object);
        }

        [Fact]
        public async Task LogPurchase_ShouldReturnOk_WhenSuccessful()
        {
            // Arrange
            var purchase = new StockTransaction { Id = 1, ProductId = 10, Quantity = 5, Type = "Purchase" };
            _saleServiceMock.Setup(s => s.LogPurchaseAsync(purchase)).ReturnsAsync(true);

            // Act
            var result = await _controller.LogPurchase(purchase);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task LogPurchase_ShouldReturnBadRequest_WhenFailed()
        {
            // Arrange
            var purchase = new StockTransaction { Id = 1, ProductId = 10, Quantity = 5, Type = "Purchase" };
            _saleServiceMock.Setup(s => s.LogPurchaseAsync(purchase)).ReturnsAsync(false);

            // Act
            var result = await _controller.LogPurchase(purchase);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task LogSale_ShouldReturnOk_WhenSuccessful()
        {
            // Arrange
            var sale = new StockTransaction { Id = 2, ProductId = 20, Quantity = 2, Type = "Sale" };
            _saleServiceMock.Setup(s => s.LogSaleAsync(sale)).ReturnsAsync(true);

            // Act
            var result = await _controller.LogSale(sale);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task LogSale_ShouldReturnBadRequest_WhenFailed()
        {
            // Arrange
            var sale = new StockTransaction { Id = 2, ProductId = 20, Quantity = 2, Type = "Sale" };
            _saleServiceMock.Setup(s => s.LogSaleAsync(sale)).ReturnsAsync(false);

            // Act
            var result = await _controller.LogSale(sale);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }
    }
}
