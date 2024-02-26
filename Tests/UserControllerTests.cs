using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;

namespace mtcg.Tests
{
    public class UserControllerTests
    {
        [Fact]
        public void RegisterUser_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            var mockContext = new Mock<mtcgDbContext>();
            var mockJwtAuthenticationService = new Mock<JwtAuthenticationService>();
            var controller = new UserController(mockContext.Object, mockJwtAuthenticationService.Object);
            var request = new UserRegistrationRequest { Username = "testuser", Password = "password" };

            // Act
            var result = controller.RegisterUser(request);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void UpdateUserProfile_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            var mockContext = new Mock<mtcgDbContext>();
            var mockJwtAuthenticationService = new Mock<JwtAuthenticationService>();
            var controller = new UserController(mockContext.Object, mockJwtAuthenticationService.Object);
            var username = "testuser";
            var userProfile = new UserProfile { Name = "UpdatedName", Bio = "UpdatedBio" };

            // Act
            var result = controller.UpdateUserProfile(username, userProfile);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetUserProfile_ExistingUser_ReturnsOkResultWithUserProfile()
        {
            // Arrange
            var mockContext = new Mock<mtcgDbContext>();
            var mockJwtAuthenticationService = new Mock<JwtAuthenticationService>();
            var controller = new UserController(mockContext.Object, mockJwtAuthenticationService.Object);
            var username = "testuser";
            var user = new User { Username = username };
            mockContext.Setup(c => c.Users.FirstOrDefault(u => u.Username == username)).Returns(user);

            // Act
            var result = controller.GetUserProfile(username) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<UserProfile>(result.Value);
            var userProfile = Assert.IsAssignableFrom<UserProfile>(result.Value);
            Assert.Equal(username, userProfile.Name);
        }

        // Add more tests for error cases, validation, etc.
    }
}
