
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace mtcg.Tests
{
    public class CardsControllerTests
    {
        [Fact]
        public void GetUserCards_ValidUser_ReturnsUserCards()
        {
            // Arrange
            var mockContext = new Mock<mtcgDbContext>();
            var mockJwtAuthenticationService = new Mock<JwtAuthenticationService>();
            var controller = new CardsController(mockContext.Object, mockJwtAuthenticationService.Object);
            var user = new User { UserId = Guid.NewGuid(), Username = "testuser" };
            var userCards = new List<Card>
            {
                new Card { OwnerId = user.UserId, Name = "Card1" },
                new Card { OwnerId = user.UserId, Name = "Card2" },
                new Card { OwnerId = user.UserId, Name = "Card3" },
                new Card { OwnerId = user.UserId, Name = "Card4" }
            };
            mockJwtAuthenticationService.Setup(s => s.GetUsernameFromToken(It.IsAny<string>())).Returns(user.Username);
            mockContext.Setup(c => c.Users.FirstOrDefault(u => u.Username == user.Username)).Returns(user);
            mockContext.Setup(c => c.Cards.Where(c => c.OwnerId == user.UserId)).Returns(userCards.AsQueryable());

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "Bearer token";
            controller.ControllerContext = new ControllerContext { HttpContext = httpContext };

            // Act
            var result = controller.GetUserCards() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            var returnedUserCards = result.Value as List<Card>;
            Assert.NotNull(returnedUserCards);
            Assert.Equal(userCards.Count, returnedUserCards.Count);
            foreach (var card in userCards)
            {
                Assert.Contains(returnedUserCards, c => c.Name == card.Name);
            }
        }

        [Fact]
        public void GetUserCards_InvalidUser_ReturnsUnauthorized()
        {
            // Arrange
            var mockContext = new Mock<mtcgDbContext>();
            var mockJwtAuthenticationService = new Mock<JwtAuthenticationService>();
            var controller = new CardsController(mockContext.Object, mockJwtAuthenticationService.Object);
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "Bearer token";
            controller.ControllerContext = new ControllerContext { HttpContext = httpContext };

            // Act
            var result = controller.GetUserCards() as UnauthorizedResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(401, result.StatusCode);
        }

        // Add more tests for other scenarios
    }
}
