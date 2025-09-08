using API.Controllers;
using Core.IServices;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Tests
{
    public class AuditLogsControllerTests
    {
        private readonly Mock<IAuditLogService> _auditLogServiceMock;
        private readonly AuditLogsController _controller;

        public AuditLogsControllerTests()
        {
            _auditLogServiceMock = new Mock<IAuditLogService>();
            _controller = new AuditLogsController(_auditLogServiceMock.Object);
        }

        [Fact]
        public async Task GetAllLogs_ShouldReturnOk_WithLogs()
        {
            // Arrange
            var logs = new List<AuditLog> { new AuditLog { Id = 1, Action = "Added Product", UserId = "1" } };
            _auditLogServiceMock.Setup(s => s.GetAllLogsAsync()).ReturnsAsync(logs);

            // Act
            var result = await _controller.GetAllLogs();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(logs, okResult.Value);
        }

        [Fact]
        public async Task GetLogById_ShouldReturnOk_WhenLogExists()
        {
            // Arrange
            var log = new AuditLog { Id = 1, Action = "Added Product", UserId = "1" };
            _auditLogServiceMock.Setup(s => s.GetLogByIdAsync(1)).ReturnsAsync(log);

            // Act
            var result = await _controller.GetLogById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(log, okResult.Value);
        }

        [Fact]
        public async Task GetLogById_ShouldReturnNotFound_WhenLogDoesNotExist()
        {
            // Arrange
            _auditLogServiceMock.Setup(s => s.GetLogByIdAsync(99)).ReturnsAsync((AuditLog)null);

            // Act
            var result = await _controller.GetLogById(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteLog_ShouldReturnOk_WhenSuccessful()
        {
            // Arrange
            _auditLogServiceMock.Setup(s => s.DeleteLogAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteLog(1);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DeleteLog_ShouldReturnNotFound_WhenFailed()
        {
            // Arrange
            _auditLogServiceMock.Setup(s => s.DeleteLogAsync(99)).ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteLog(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
