using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using XunitAssert = Xunit.Assert;

namespace mtcg.Tests
{
    public class SessionsControllerTests
    {
        [Fact]
        public void Login_ValidCredentials_ReturnsOkResultWithToken()
        {
            // Arrange
            var mockContext = new Mock<mtcgDbContext>();
            var mockJwtAuthenticationService = new Mock<JwtAuthenticationService>();
            var controller = new SessionsController(mockContext.Object, mockJwtAuthenticationService.Object);
            var credentials = new UserCredentials { Username = "testuser", Password = "password" };
            mockContext.Setup(c => c.Users.FirstOrDefault(u => u.Username == credentials.Username && u.Password == credentials.Password))
                       .Returns(new User { Username = credentials.Username, Password = credentials.Password });
            mockJwtAuthenticationService.Setup(s => s.GenerateJwtToken(credentials.Username)).Returns("test_token");

            // Act
            var result = controller.Login(credentials) as OkObjectResult;

            // Assert
            XunitAssert.NotNull(result);
            XunitAssert.Equal("test_token", result.Value);
        }

    }
}
