using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace mtcg.Tests
{
    public class TradingControllerTests
    {
        [Fact]
        public void GetTradingDeals_ReturnsAvailableDeals()
        {
            // Arrange
            var mockContext = new Mock<mtcgDbContext>();
            var mockJwtAuthenticationService = new Mock<JwtAuthenticationService>();
            var controller = new TradingController(mockContext.Object, mockJwtAuthenticationService.Object);
            var tradingDeals = new List<TradingDeal> { new TradingDeal(), new TradingDeal() };
            mockContext.Setup(c => c.TradingDeals.ToList()).Returns(tradingDeals);

            // Act
            var result = controller.GetTradingDeals() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(tradingDeals, result.Value);
        }

        [Fact]
        public void PostTradingDeal_ValidDeal_ReturnsCreatedResult()
        {
            // Arrange
            var mockContext = new Mock<mtcgDbContext>();
            var mockJwtAuthenticationService = new Mock<JwtAuthenticationService>();
            var controller = new TradingController(mockContext.Object, mockJwtAuthenticationService.Object);
            var tradingDeal = new TradingDeal { Id = Guid.NewGuid(), CardToTrade = Guid.NewGuid(), CreatorUsername = "testuser" };

            // Act
            var result = controller.PostTradingDeal(tradingDeal) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(201, result.StatusCode);
            Assert.Equal("Trading deal successfully created.", result.Value);
            mockContext.Verify(c => c.SaveChanges(), Times.Once);
        }

        [Fact]
        public void DeleteTradingDeal_ValidDeal_ReturnsOkResult()
        {
            // Arrange
            var mockContext = new Mock<mtcgDbContext>();
            var mockJwtAuthenticationService = new Mock<JwtAuthenticationService>();
            var controller = new TradingController(mockContext.Object, mockJwtAuthenticationService.Object);
            var tradingDealId = Guid.NewGuid();
            var tradingDeal = new TradingDeal { Id = tradingDealId, CreatorUsername = "testuser" };
            mockContext.Setup(c => c.TradingDeals.FirstOrDefault(td => td.Id == tradingDealId && td.CreatorUsername == "testuser")).Returns(tradingDeal);

            // Act
            var result = controller.DeleteTradingDeal(tradingDealId) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("Trading deal successfully deleted.", result.Value);
            mockContext.Verify(c => c.TradingDeals.Remove(tradingDeal), Times.Once);
            mockContext.Verify(c => c.SaveChanges(), Times.Once);
        }

        [Fact]
        public void TradeWithDeal_ValidTrade_ReturnsOkResult()
        {
            // Arrange
            var mockContext = new Mock<mtcgDbContext>();
            var mockJwtAuthenticationService = new Mock<JwtAuthenticationService>();
            var controller = new TradingController(mockContext.Object, mockJwtAuthenticationService.Object);
            var tradingDealId = Guid.NewGuid();
            var cardId = Guid.NewGuid();
            var tradingDeal = new TradingDeal { Id = tradingDealId, CardToTrade = cardId };
            var cardToTrade = new Card { Id = cardId, OwnerId = Guid.NewGuid() }; // Assuming the card is owned by a different user
            mockContext.Setup(c => c.TradingDeals.FirstOrDefault(td => td.Id == tradingDealId)).Returns(tradingDeal);
            mockContext.Setup(c => c.Cards.FirstOrDefault(c => c.Id == cardId)).Returns(cardToTrade);

            // Act
            var result = controller.TradeWithDeal(tradingDealId, cardId) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("Trading deal successfully executed. Card acquired.", result.Value);
            Assert.Equal(cardToTrade.OwnerId, Guid.Empty); // Assert that the card is now owned by the current user
            mockContext.Verify(c => c.SaveChanges(), Times.Once);
        }
    }
}
