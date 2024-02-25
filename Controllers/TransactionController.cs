using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mtcg;

[ApiController]
[Route("transactions")]
public class TransactionController : ControllerBase
{
    private readonly mtcgDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly JwtAuthenticationService _jwtAuthenticationService;
    private readonly PackageService _packageService;


    public TransactionController(mtcgDbContext context, IHttpContextAccessor httpContextAccessor, JwtAuthenticationService jwtAuthenticationService, PackageService packageService)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _jwtAuthenticationService = jwtAuthenticationService;
        _packageService = packageService;
    }

    [HttpPost("packages")]
    [Authorize] // Require a valid JWT token to access this endpoint
    public IActionResult PurchasePackage()
    {
        // Get the username from the JWT token
        var username = _jwtAuthenticationService.GetUsernameFromToken(HttpContext.Request.Headers["Authorization"]);

        // Get the current user from the database based on the username
        var currentUser = _context.Users.FirstOrDefault(u => u.Username == username);

        if (currentUser == null)
        {
            return Unauthorized(); // User not found or not authenticated
        }

        // Check if there are available packages
        var availablePackage = _context.Packages.FirstOrDefault(p => p.OwnerId == null); // Assuming only one package can be purchased at a time

        if (availablePackage == null)
        {
            return NotFound("No card package available for buying!");
        }

        // Check if the user has enough coins to buy this package
        if (currentUser.Coins < availablePackage.Price)
        {
            return BadRequest("Insufficient coins to purchase this package!");
        }

        // Deduct the price from the user's coins
        currentUser.Coins -= availablePackage.Price;

        // Assign the package to the user
        availablePackage.OwnerId = currentUser.UserId;

        // Get the cards contained in the purchased package
        var packageCards = availablePackage.Cards;

        // Update the OwnerId of each card to match the package's OwnerId
        foreach (var card in packageCards)
        {
            card.OwnerId = availablePackage.OwnerId;
        }

        // Save changes to the database
        _context.SaveChanges();

        // Return the updated coins left
        return Ok(new { CoinsLeft = currentUser.Coins });
    }


}
