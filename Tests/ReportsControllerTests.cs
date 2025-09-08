using API.Controllers;
using Core.IServices;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Tests
{
    public class ReportsControllerTests
    {
        private readonly Mock<IReportingService> _reportServiceMock;
        private readonly ReportsController _controller;

        public ReportsControllerTests()
        {
            _reportServiceMock = new Mock<IReportingService>();
            _controller = new ReportsController(_reportServiceMock.Object);
        }

        [Fact]
        public async Task Purchases_ShouldReturnOk_WithTransactions()
        {
            // Arrange
            var purchases = new List<StockTransaction> { new StockTransaction { Id = 1, Type = "Purchase", Quantity = 10 } };
            _reportServiceMock.Setup(s => s.GetPurchasesAsync()).ReturnsAsync(purchases);

            // Act
            var result = await _controller.Purchases();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnData = Assert.IsType<List<StockTransaction>>(okResult.Value);
            Assert.Single(returnData);
        }

        [Fact]
        public async Task Sales_ShouldReturnOk_WithTransactions()
        {
            // Arrange
            var sales = new List<StockTransaction> { new StockTransaction { Id = 2, Type = "Sale", Quantity = 5 } };
            _reportServiceMock.Setup(s => s.GetSalesAsync()).ReturnsAsync(sales);

            // Act
            var result = await _controller.Sales();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnData = Assert.IsType<List<StockTransaction>>(okResult.Value);
            Assert.Single(returnData);
        }

        [Fact]
        public async Task AuditLogs_ShouldReturnOk_WithLogs()
        {
            // Arrange
            var logs = new List<AuditLog> { new AuditLog { Id = 1, Action = "Product Created" } };
            _reportServiceMock.Setup(s => s.GetAuditLogsAsync()).ReturnsAsync(logs);

            // Act
            var result = await _controller.AuditLogs();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnData = Assert.IsType<List<AuditLog>>(okResult.Value);
            Assert.Single(returnData);
        }
    }
}
