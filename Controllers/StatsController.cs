using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mtcg;


[ApiController]
[Route("stats")]
public class StatsController : ControllerBase
{
    private readonly mtcgDbContext _context;
    private readonly JwtAuthenticationService _jwtAuthenticationService;

    public StatsController(mtcgDbContext context, JwtAuthenticationService jwtAuthenticationService)
    {
        _context = context;
        _jwtAuthenticationService = jwtAuthenticationService;
    }

    [HttpGet]
    [Authorize] // Require a valid JWT token to access this endpoint
    public IActionResult GetUserStats()
    {
        try
        {
            var username = _jwtAuthenticationService.GetUsernameFromToken(HttpContext.Request.Headers["Authorization"]);
            var user = _context.Users.FirstOrDefault(u => u.Username == username);

            if (user == null)
            {
                return Unauthorized();
            }

            var userStats = GetUserStatsFromDatabase(user);

            return Ok(userStats);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while retrieving user stats." });
        }
    }

    private UserStats GetUserStatsFromDatabase(User user)
    {
        return _context.UserStats.FirstOrDefault(stats => stats.UserId == user.UserId);
    }
}
