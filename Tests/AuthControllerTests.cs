using API.Controllers;
using API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Tests
{
    public class AuthControllerTests
    {
        private readonly Mock<IAuthService> _authServiceMock;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _authServiceMock = new Mock<IAuthService>();
            _controller = new AuthController(_authServiceMock.Object);
        }

        [Fact]
        public async Task Register_ShouldReturnOk_WhenSuccessful()
        {
            // Arrange
            var dto = new RegisterDto { FullName = "Test User", Email = "test@example.com", Password = "Pass123$", Role = "Staff" };
            _authServiceMock.Setup(s => s.RegisterAsync(dto)).ReturnsAsync(true);

            // Act
            var result = await _controller.Register(dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task Register_ShouldReturnBadRequest_WhenFailed()
        {
            // Arrange
            var dto = new RegisterDto { FullName = "Test User", Email = "test@example.com", Password = "Pass123$", Role = "Staff" };
            _authServiceMock.Setup(s => s.RegisterAsync(dto)).ReturnsAsync(false);

            // Act
            var result = await _controller.Register(dto);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequest.StatusCode);
        }

        [Fact]
        public async Task Login_ShouldReturnOk_WhenCredentialsValid()
        {
            // Arrange
            var dto = new LoginDto { Email = "test@example.com", Password = "Pass123$" };
            _authServiceMock.Setup(s => s.LoginAsync(dto)).ReturnsAsync("mock-token");

            // Act
            var result = await _controller.Login(dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public async Task Login_ShouldReturnUnauthorized_WhenInvalid()
        {
            // Arrange
            var dto = new LoginDto { Email = "wrong@example.com", Password = "wrongpass" };
            _authServiceMock.Setup(s => s.LoginAsync(dto)).ReturnsAsync((string)null);

            // Act
            var result = await _controller.Login(dto);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(result);
        }
    }
}
