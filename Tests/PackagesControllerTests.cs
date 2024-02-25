using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using mtcg;
using Moq;
using Xunit;

using XunitAssert = Xunit.Assert;

namespace mtcg.Tests
{
    public class PackagesControllerTests
    {
        [Fact]
        public void CreatePackage_ValidPackage_ReturnsCreatedResult()
        {
            // Arrange
            var mockPackageService = new Mock<IPackageService>();
            var controller = new PackagesController(mockPackageService.Object);
            var cards = new List<Card>
            {
                new Card(), new Card(), new Card(), new Card(), new Card()
            };
            mockPackageService.Setup(s => s.CardsExist(cards)).Returns(false); // Simulate no existing cards

            // Act
            var result = controller.CreatePackage(cards) as CreatedResult;

            // Assert
            XunitAssert.NotNull(result);
            XunitAssert.Equal(201, result.StatusCode);
            XunitAssert.Equal("Package and cards successfully created", result.Value);
        }

        [Fact]
        public void CreatePackage_InvalidPackage_ReturnsBadRequestResult()
        {
            // Arrange
            var mockPackageService = new Mock<IPackageService>();
            var controller = new PackagesController(mockPackageService.Object);
            var cards = new List<Card>(); // Package with no cards

            // Act
            var result = controller.CreatePackage(cards) as BadRequestObjectResult;

            // Assert
            XunitAssert.NotNull(result);
            XunitAssert.Equal(400, result.StatusCode);
            XunitAssert.Equal("A package must contain exactly 5 cards", result.Value);
        }

        [Fact]
        public void CreatePackage_WithInvalidNumberOfCards_ReturnsBadRequest()
        {
            // Arrange
            var mockPackageService = new Mock<IPackageService>();
            var controller = new PackagesController(mockPackageService.Object);
            var cards = new List<Card>
            {
                new Card { Id = Guid.NewGuid(), Name = "Card1" },
                new Card { Id = Guid.NewGuid(), Name = "Card2" },
                new Card { Id = Guid.NewGuid(), Name = "Card3" },
                new Card { Id = Guid.NewGuid(), Name = "Card4" } // Missing one card
            };

            // Act
            var result = controller.CreatePackage(cards) as BadRequestObjectResult;

            // Assert
            XunitAssert.NotNull(result);
            XunitAssert.Equal(400, result.StatusCode);
            XunitAssert.Equal("A package must contain exactly 5 cards", result.Value);
        }

    [Fact]
        public void CreatePackage_CardsExistInDatabase_ReturnsConflictResult()
        {
            // Arrange
            var mockPackageService = new Mock<IPackageService>();
            var controller = new PackagesController(mockPackageService.Object);
            var existingCardId = Guid.NewGuid(); // Assume this card ID already exists in the database
            var cards = new List<Card>
            {
                new Card { Id = existingCardId }, // Add an existing card
                new Card(), new Card(), new Card(), new Card() // Add other cards to make a total of 5
            };
            mockPackageService.Setup(s => s.CardsExist(cards)).Returns(true); // Simulate that cards already exist in the database

            // Act
            var result = controller.CreatePackage(cards) as ConflictObjectResult;

            // Assert
            XunitAssert.NotNull(result);
            XunitAssert.Equal(409, result.StatusCode);
            XunitAssert.Equal("At least one card in the package already exists", result.Value);
        }

        // Add more tests for error cases, validation, etc.
    }
}
