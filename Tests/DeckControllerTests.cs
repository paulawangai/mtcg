using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace mtcg.Tests
{
    public class DeckControllerTests
    {
        [Fact]
        public void GetUserDeck_UserWithConfiguredDeck_ReturnsOkResult()
        {
            // Arrange
            var mockContext = new Mock<mtcgDbContext>();
            var mockJwtAuthenticationService = new Mock<JwtAuthenticationService>();
            var controller = new DeckController(mockContext.Object, mockJwtAuthenticationService.Object);
            var username = "testuser";
            var currentUser = new User { Username = username };
            currentUser.ConfiguredDeck = new UserDeck { Cards = new List<Card> { new Card(), new Card(), new Card(), new Card() } };
            mockContext.Setup(c => c.Users.FirstOrDefault(u => u.Username == username)).Returns(currentUser);

            // Act
            var result = controller.GetUserDeck() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Card>>(result.Value);
            var userDeck = Assert.IsAssignableFrom<List<Card>>(result.Value);
            Assert.Equal(4, userDeck.Count);
        }

        [Fact]
        public void GetUserDeck_UserWithoutConfiguredDeck_ReturnsOkResultWithUserCards()
        {
            // Arrange
            var mockContext = new Mock<mtcgDbContext>();
            var mockJwtAuthenticationService = new Mock<JwtAuthenticationService>();
            var controller = new DeckController(mockContext.Object, mockJwtAuthenticationService.Object);
            var username = "testuser";
            var currentUser = new User { Username = username };
            currentUser.ConfiguredDeck = null;
            var userCards = new List<Card> { new Card(), new Card(), new Card(), new Card() };
            mockContext.Setup(c => c.Users.FirstOrDefault(u => u.Username == username)).Returns(currentUser);
            mockContext.Setup(c => c.Cards.Where(c => c.OwnerId == currentUser.UserId).Take(4)).Returns(userCards.AsQueryable);

            // Act
            var result = controller.GetUserDeck() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Card>>(result.Value);
            var userDeck = Assert.IsAssignableFrom<List<Card>>(result.Value);
            Assert.Equal(4, userDeck.Count);
            Assert.Equal(userCards, userDeck);
        }

        [Fact]
        public void ConfigureUserDeck_InvalidCardIds_ReturnsBadRequest()
        {
            // Arrange
            var mockContext = new Mock<mtcgDbContext>();
            var mockJwtAuthenticationService = new Mock<JwtAuthenticationService>();
            var controller = new DeckController(mockContext.Object, mockJwtAuthenticationService.Object);
            var username = "testuser";
            var currentUser = new User { Username = username };
            mockContext.Setup(c => c.Users.FirstOrDefault(u => u.Username == username)).Returns(currentUser);
            var cardIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() }; // Invalid: not enough card IDs

            // Act
            var result = controller.ConfigureUserDeck(cardIds) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Please provide exactly 4 card IDs to configure your deck.", result.Value);
        }

        [Fact]
        public void ConfigureUserDeck_ValidCardIds_ReturnsOkResult()
        {
            // Arrange
            var mockContext = new Mock<mtcgDbContext>();
            var mockJwtAuthenticationService = new Mock<JwtAuthenticationService>();
            var controller = new DeckController(mockContext.Object, mockJwtAuthenticationService.Object);
            var username = "testuser";
            var currentUser = new User { Username = username };
            mockContext.Setup(c => c.Users.FirstOrDefault(u => u.Username == username)).Returns(currentUser);
            var cardIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() }; // Valid: exactly 4 card IDs
            var cards = cardIds.Select(id => new Card { Id = id, OwnerId = currentUser.UserId }).ToList();
            mockContext.Setup(c => c.Cards.Where(c => c.OwnerId == currentUser.UserId && cardIds.Contains(c.Id))).Returns(cards.AsQueryable);

            // Act
            var result = controller.ConfigureUserDeck(cardIds) as OkResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(currentUser.ConfiguredDeck);
            Assert.Equal(cards, currentUser.ConfiguredDeck.Cards);
        }

    }
}
