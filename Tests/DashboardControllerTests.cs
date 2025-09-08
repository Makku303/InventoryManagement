using API.Controllers;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class DashboardControllerTests
    {
        private readonly Mock<IDashboardService> _dashboardServiceMock;
        private readonly DashboardController _controller;

        public DashboardControllerTests()
        {
            _dashboardServiceMock = new Mock<IDashboardService>();
            _controller = new DashboardController(_dashboardServiceMock.Object);
        }

        [Fact]
        public async Task LowStock_ShouldReturnOk_WithProducts()
        {
            // Arrange
            var products = new List<Product> { new Product { Id = 1, Name = "Product A", Quantity = 2 } };
            _dashboardServiceMock.Setup(s => s.GetLowStockProductsAsync()).ReturnsAsync(products);

            // Act
            var result = await _controller.LowStock();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnProducts = Assert.IsType<List<Product>>(okResult.Value);
            Assert.Single(returnProducts);
        }

        [Fact]
        public async Task OutOfStock_ShouldReturnOk_WithProducts()
        {
            // Arrange
            var products = new List<Product> { new Product { Id = 2, Name = "Product B", Quantity = 0 } };
            _dashboardServiceMock.Setup(s => s.GetOutOfStockProductsAsync()).ReturnsAsync(products);

            // Act
            var result = await _controller.OutOfStock();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnProducts = Assert.IsType<List<Product>>(okResult.Value);
            Assert.Single(returnProducts);
        }

        [Fact]
        public async Task BestSellers_ShouldReturnOk_WithProducts()
        {
            // Arrange
            var products = new List<Product> { new Product { Id = 3, Name = "Product C", Quantity = 50 } };
            _dashboardServiceMock.Setup(s => s.GetBestSellersAsync()).ReturnsAsync(products);

            // Act
            var result = await _controller.BestSellers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnProducts = Assert.IsType<List<Product>>(okResult.Value);
            Assert.Single(returnProducts);
        }

        [Fact]
        public async Task RecentActivity_ShouldReturnOk_WithActivity()
        {
            // Arrange
            var purchases = new List<StockTransaction> { new StockTransaction { Id = 1, Type = "Purchase" } };
            var sales = new List<StockTransaction> { new StockTransaction { Id = 2, Type = "Sale" } };

            _dashboardServiceMock.Setup(s => s.GetRecentPurchasesAsync()).ReturnsAsync(purchases);
            _dashboardServiceMock.Setup(s => s.GetRecentSalesAsync()).ReturnsAsync(sales);

            // Act
            var result = await _controller.RecentActivity();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var activity = okResult.Value as dynamic;
            Assert.NotNull(activity);
            Assert.Single(activity.Purchases);
            Assert.Single(activity.Sales);
        }
    }
}
