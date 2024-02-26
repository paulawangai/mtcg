using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mtcg;


[ApiController]
[Route("deck")]
public class DeckController : ControllerBase
{
    private readonly mtcgDbContext _context;
    private readonly JwtAuthenticationService _jwtAuthenticationService;

    public DeckController(mtcgDbContext context, JwtAuthenticationService jwtAuthenticationService)
    {
        _context = context;
        _jwtAuthenticationService = jwtAuthenticationService;
    }

    [HttpGet]
    [Authorize]
    public IActionResult GetUserDeck()
    {
        var username = _jwtAuthenticationService.GetUsernameFromToken(HttpContext.Request.Headers["Authorization"]);
        var currentUser = _context.Users.FirstOrDefault(u => u.Username == username);

        if (currentUser == null)
        {
            return Unauthorized();
        }

        List<Card> userDeckCards;

        if (currentUser.ConfiguredDeck != null)
        {
            userDeckCards = currentUser.ConfiguredDeck.Cards.Take(4).ToList(); // Get all cards from the configured deck of the current user
        }
        else
        {
            userDeckCards = _context.Cards.Where(c => c.OwnerId == currentUser.UserId).Take(4).ToList(); // Take only the first 4 cards from the user's card collection
        }

        return Ok(userDeckCards);
    }


    [HttpPut]
    [Authorize] // Require a valid JWT token to access this endpoint
    public IActionResult ConfigureUserDeck([FromBody] List<Guid> cardIds)
    {
        var username = _jwtAuthenticationService.GetUsernameFromToken(HttpContext.Request.Headers["Authorization"]);
        var currentUser = _context.Users.FirstOrDefault(u => u.Username == username);

        if (currentUser == null)
        {
            return Unauthorized();
        }

        var selectedCards = _context.Cards.Where(c => c.OwnerId == currentUser.UserId && cardIds.Contains(c.Id)).ToList();

        if (selectedCards.Count != 4)
        {
            return BadRequest("Please provide exactly 4 card IDs to configure your deck.");
        }

        currentUser.ConfiguredDeck = new UserDeck
        {
            Cards = selectedCards
        };

        _context.SaveChanges();

        return Ok();
    }
}
