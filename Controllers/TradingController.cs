using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mtcg;

[ApiController]
[Route("tradings")]
public class TradingController : ControllerBase
{
    private readonly mtcgDbContext _context;
    private readonly JwtAuthenticationService _jwtAuthenticationService;

    public TradingController(mtcgDbContext context, JwtAuthenticationService jwtAuthenticationService)
    {
        _context = context;
        _jwtAuthenticationService = jwtAuthenticationService;
    }

    [HttpGet]
    [Authorize]
    public IActionResult GetTradingDeals()
    {
        var username = _jwtAuthenticationService.GetUsernameFromToken(HttpContext.Request.Headers["Authorization"]);
        var tradingDeals = GetAvailableTradingDeals();
        if (tradingDeals.Count == 0)
        {
            return StatusCode(204);
        }

        return Ok(tradingDeals);
    }

    [HttpPost]
    [Authorize]
    public IActionResult PostTradingDeal([FromBody] TradingDeal tradingDeal)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var username = _jwtAuthenticationService.GetUsernameFromToken(HttpContext.Request.Headers["Authorization"]);
        var currentUser = _context.Users.FirstOrDefault(u => u.Username == username);
        if (currentUser == null)
        {
            return Unauthorized();
        }

        if (!IsCardOwnedByUser(username, tradingDeal.CardToTrade))
        {
            return StatusCode(403, "The card to trade is not owned by the user.");
        }

        if (IsCardLockedInDeck(username, tradingDeal.CardToTrade))
        {
            return StatusCode(403, "The card to trade is locked in the deck.");
        }

        if (IsTradingDealExists(tradingDeal.Id))
        {
            return StatusCode(409, "A deal with this ID already exists.");
        }

        // Store the username of the creator in the trading deal
        tradingDeal.CreatorUsername = username;

        _context.TradingDeals.Add(tradingDeal);
        _context.SaveChanges();

        return StatusCode(201, "Trading deal successfully created.");
    }


    [HttpDelete("{tradingDealId}")]
    [Authorize]
    public IActionResult DeleteTradingDeal(Guid tradingDealId)
    {
        var username = _jwtAuthenticationService.GetUsernameFromToken(HttpContext.Request.Headers["Authorization"]);
        var currentUser = _context.Users.FirstOrDefault(u => u.Username == username);
        if (currentUser == null)
        {
            return Unauthorized();
        }

        // Check if the user is authorized to delete the deal
        var tradingDeal = _context.TradingDeals.FirstOrDefault(td => td.Id == tradingDealId && td.CreatorUsername == username);
        if (tradingDeal == null)
        {
            return NotFound("Trading deal not found.");
        }

        _context.TradingDeals.Remove(tradingDeal);
        _context.SaveChanges();

        return Ok("Trading deal successfully deleted.");
    }



    [HttpPost("{tradingDealId}")]
    [Authorize]
    public IActionResult TradeWithDeal(Guid tradingDealId, [FromBody] Guid cardId)
    {
        var username = _jwtAuthenticationService.GetUsernameFromToken(HttpContext.Request.Headers["Authorization"]);
        var currentUser = _context.Users.FirstOrDefault(u => u.Username == username);
        if (currentUser == null)
        {
            return Unauthorized();
        }

        // Check if the user is trying to trade with themselves
        if (IsTradingWithSelf(username, tradingDealId, cardId))
        {
            return BadRequest("Trading with yourself is not allowed.");
        }

        // Retrieve the trading deal based on the provided tradingDealId
        var tradingDeal = _context.TradingDeals.FirstOrDefault(td => td.Id == tradingDealId);
        if (tradingDeal == null)
        {
            return NotFound("Trading deal not found.");
        }

        // Retrieve the card associated with the trading deal
        var cardToTrade = _context.Cards.FirstOrDefault(c => c.Id == tradingDeal.CardToTrade);
        if (cardToTrade == null)
        {
            return NotFound("Card to trade not found.");
        }

        // Check if the trading deal requirements are met
        if (!IsCardEligibleForTrade(tradingDeal))
        {
            return BadRequest("The trading deal requirements are not met.");
        }

        // Check if the offered card meets the trading deal requirements
        if (!IsOfferedCardEligibleForTrade(tradingDealId, cardId))
        {
            return BadRequest("The offered card does not meet the trading deal requirements.");
        }

        // Assign the acquired card to the current user
        cardToTrade.OwnerId = currentUser.UserId;
        _context.SaveChanges();

        // Perform any additional logic related to the trade operation here

        return Ok("Trading deal successfully executed. Card acquired.");
    }



    // Helper methods for validations and checks
    private List<TradingDeal> GetAvailableTradingDeals()
    {
        return _context.TradingDeals.ToList(); // Or implement your own logic to filter available deals
    }

    private bool IsCardOwnedByUser(string username, Guid cardId)
    {
        var currentUser = _context.Users.FirstOrDefault(u => u.Username == username);
        if (currentUser == null)
        {
            return false;
        }

        // Check if the card belongs to the authenticated user
        var userCard = _context.Cards.FirstOrDefault(c => c.Id == cardId && c.OwnerId == currentUser.UserId);
        return userCard != null;
    }


    private bool IsCardEligibleForTrade(TradingDeal tradingDeal)
    {
        if (tradingDeal == null)
        {
            // The trading deal object is null, which means it's invalid
            return false;
        }

        // Get the card to trade
        var cardToTrade = _context.Cards.FirstOrDefault(c => c.Id == tradingDeal.CardToTrade);
        if (cardToTrade == null)
        {
            // The card to trade does not exist
            return false;
        }

        // Convert DealType to CardType enum for comparison
        CardType dealTypeAsCardType;
        if (!Enum.TryParse(tradingDeal.Type.ToString(), out dealTypeAsCardType))
        {
            // Invalid deal type
            return false;
        }

        // Check if the card type matches the trading deal type
        if (cardToTrade.Type != (CardType)tradingDeal.Type)
        {
            return false;
        }

        // Check if the card's damage meets the minimum damage requirement of the trading deal
        if (cardToTrade.Damage < tradingDeal.MinimumDamage)
        {
            return false;
        }

        return true;
    }



    private bool IsTradingDealExists(Guid tradingDealId)
    {
        // Check if a trading deal with the provided ID exists in the database
        var tradingDeal = _context.TradingDeals.FirstOrDefault(td => td.Id == tradingDealId);

        // If a trading deal is found, return true; otherwise, return false
        return tradingDeal != null;
    }


    private bool IsUserAuthorizedToDeleteDeal(string username, Guid tradingDealId)
    {
        var currentUser = _context.Users.FirstOrDefault(u => u.Username == username);
        if (currentUser == null)
        {
            // User not found, so they cannot own any cards
            return false;
        }

        var tradingDeal = _context.TradingDeals.FirstOrDefault(td => td.Id == tradingDealId);
        if (tradingDeal == null)
        {
            // Trading deal not found, user cannot delete a non-existing deal
            return false;
        }

        // Check if the user owns the card associated with the trading deal
        return IsCardOwnedByUser(username, (Guid)tradingDeal.CardToTrade);

    }


    private bool IsTradingWithSelf(string username, Guid tradingDealId, Guid offeredCardId)
    {
        // Get the owner ID of the card being traded
        var cardOwnerUserId = _context.Cards
            .Where(c => c.Id == offeredCardId)
            .Select(c => c.OwnerId)
            .FirstOrDefault();

        // Get the user ID associated with the provided username
        var currentUser = _context.Users
            .FirstOrDefault(u => u.Username == username);

        // Compare the owner ID of the card with the ID of the current user
        return cardOwnerUserId == currentUser?.UserId;
    }


    private bool IsCardLockedInDeck(string username, Guid cardId)
    {
        var currentUser = _context.Users.FirstOrDefault(u => u.Username == username);
        if (currentUser == null || currentUser.ConfiguredDeck == null)
        {
            // User not found or user doesn't have a configured deck
            return false;
        }

        // Check if the card is present in the user's configured deck
        return currentUser.ConfiguredDeck.Cards.Any(c => c.Id == cardId);
    }

    private bool IsOfferedCardEligibleForTrade(Guid tradingDealId, Guid offeredCardId)
    {
        var tradingDeal = _context.TradingDeals.FirstOrDefault(td => td.Id == tradingDealId);
        if (tradingDeal == null)
        {
            // Trading deal not found, cannot proceed
            return false;
        }

        var offeredCard = _context.Cards.FirstOrDefault(c => c.Id == offeredCardId);
        if (offeredCard == null)
        {
            // Offered card not found, cannot proceed
            return false;
        }

        // Check if the offered card type matches the trading deal type
        // Check if the card type matches the trading deal type
        if (offeredCard.Type != null && tradingDeal.Type != null && offeredCard.Type.Value != (CardType)tradingDeal.Type)
        {
            return false;
        }

        // Check if the offered card damage meets the minimum damage requirement of the trading deal
        if (offeredCard.Damage < tradingDeal.MinimumDamage)
        {
            return false;
        }

        // Optionally, you can add more criteria checks here

        return true;
    }


}
