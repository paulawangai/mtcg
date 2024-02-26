using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mtcg;


[ApiController]
[Route("scoreboard")]
public class ScoreboardController : ControllerBase
{
    private readonly mtcgDbContext _context;

    public ScoreboardController(mtcgDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Authorize] // Require a valid JWT token to access this endpoint
    public IActionResult GetScoreboard()
    {
        try
        {
            var scoreboard = GetOrderedScoreboard();

            return Ok(scoreboard);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while retrieving the scoreboard." });
        }
    }

    private List<UserStats> GetOrderedScoreboard()
    {
        var scoreboard = _context.UserStats.OrderByDescending(stats => stats.Elo).ToList();
        return scoreboard;
    }
}

