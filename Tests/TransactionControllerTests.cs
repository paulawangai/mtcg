using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using mtcg;
using System;
using System.Linq;
using Xunit;

namespace mtcg.Tests
{
    public class TransactionControllerTests
    {
        [Fact]
        public void PurchasePackage_UserAuthenticatedAndSufficientCoins_ReturnsOkResult()
        {
            // Arrange
            var mockContext = new Mock<mtcgDbContext>();
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var mockJwtAuthenticationService = new Mock<JwtAuthenticationService>();
            var mockPackageService = new Mock<PackageService>();

            var controller = new TransactionController(mockContext.Object, mockHttpContextAccessor.Object, mockJwtAuthenticationService.Object, mockPackageService.Object);

            var username = "testuser";
            var currentUser = new User { Username = username, Coins = 50 }; // Assuming user has enough coins
            var availablePackage = new Package { Id = Guid.NewGuid(), Price = 10, Cards = new List<Card>() }; // Assuming available package exists
            var packageCards = Enumerable.Range(1, 5).Select(i => new Card { Id = Guid.NewGuid(), OwnerId = null }).ToList(); // Assuming package contains 5 cards

            mockContext.Setup(c => c.Users.FirstOrDefault(u => u.Username == username)).Returns(currentUser);
            mockContext.Setup(c => c.Packages.FirstOrDefault(p => p.OwnerId == null)).Returns(availablePackage);
            mockContext.Setup(c => c.SaveChanges()).Verifiable();

            // Act
            var result = controller.PurchasePackage() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(result.Value);
            var coinsLeft = (int)result.Value.GetType().GetProperty("CoinsLeft").GetValue(result.Value);
            Assert.Equal(40, coinsLeft); // Assuming 10 coins were deducted (50 - 10)
            mockContext.Verify(c => c.SaveChanges(), Times.Once);
        }

        [Fact]
        public void PurchasePackage_UserAuthenticatedAndInsufficientCoins_ReturnsBadRequestResult()
        {
            // Arrange
            var mockContext = new Mock<mtcgDbContext>();
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var mockJwtAuthenticationService = new Mock<JwtAuthenticationService>();
            var mockPackageService = new Mock<PackageService>();

            var controller = new TransactionController(mockContext.Object, mockHttpContextAccessor.Object, mockJwtAuthenticationService.Object, mockPackageService.Object);

            var username = "testuser";
            var currentUser = new User { Username = username, Coins = 5 }; // Assuming user has insufficient coins
            var availablePackage = new Package { Id = Guid.NewGuid(), Price = 10, Cards = new List<Card>() }; // Assuming available package exists

            mockContext.Setup(c => c.Users.FirstOrDefault(u => u.Username == username)).Returns(currentUser);
            mockContext.Setup(c => c.Packages.FirstOrDefault(p => p.OwnerId == null)).Returns(availablePackage);

            // Act
            var result = controller.PurchasePackage() as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Insufficient coins to purchase this package!", result.Value);
            mockContext.Verify(c => c.SaveChanges(), Times.Never); // Verify that SaveChanges is not called
        }

    }
}
