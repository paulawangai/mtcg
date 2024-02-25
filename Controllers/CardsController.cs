using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mtcg;

[ApiController]
[Route("cards")]
public class CardsController : ControllerBase
{
    private readonly mtcgDbContext _context;
    private readonly JwtAuthenticationService _jwtAuthenticationService;

    public CardsController(mtcgDbContext context, JwtAuthenticationService jwtAuthenticationService)
    {
        _context = context;
        _jwtAuthenticationService = jwtAuthenticationService;
    }

    [HttpGet]
    [Authorize] // Require a valid JWT token to access this endpoint
    public IActionResult GetUserCards()
    {
        // Get the username from the JWT token
        var username = _jwtAuthenticationService.GetUsernameFromToken(HttpContext.Request.Headers["Authorization"]);

        // Get the current user from the database based on the username
        var currentUser = _context.Users.FirstOrDefault(u => u.Username == username);

        if (currentUser == null)
        {
            return Unauthorized(); // User not found or not authenticated
        }

        // Get all cards acquired by the user
        var userCards = _context.Cards.Where(c => c.OwnerId == currentUser.UserId).Take(4).ToList();

        if (userCards.Any())
        {
            return Ok(userCards); // Return the user's cards if found
        }
        else
        {
            return NoContent(); // User has no cards
        }
    }
}
