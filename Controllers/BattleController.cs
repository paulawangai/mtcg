using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using mtcg.Services;

namespace mtcg.Controllers
{
    [ApiController]
    [Route("battles")]
    public class BattleController : ControllerBase
    {
        private readonly IBattleService _battleService;
        private readonly mtcgDbContext _context;
        private readonly JwtAuthenticationService _jwtAuthenticationService;

        public BattleController(IBattleService battleService, mtcgDbContext context, JwtAuthenticationService jwtAuthenticationService)
        {
            _battleService = battleService;
            _context = context;
            _jwtAuthenticationService = jwtAuthenticationService;
        }

        [HttpPost]
        [Authorize] // Requires authorization to access this endpoint
        public async Task<IActionResult> StartBattleAsync()
        {
            try
            {
                string user1Id = null;
                string user2Id = null;

                // Extract user ID from JWT token of the first API request
                user1Id = _jwtAuthenticationService.GetUsernameFromToken(HttpContext.Request.Headers["Authorization"]);
                var user1 = await _context.Users.FirstOrDefaultAsync(u => u.Username == user1Id);

                // Wait for the second API request to finish and retrieve the user ID
                await Task.Delay(1000); // Adjust delay as needed to ensure the second request is processed
                user2Id = _jwtAuthenticationService.GetUsernameFromToken(HttpContext.Request.Headers["Authorization"]);
                var user2 = await _context.Users.FirstOrDefaultAsync(u => u.Username == user2Id);

                // Check if both user IDs are available
                if (user1Id != null && user2Id != null)
                {
                    // Get users from database
                    if (user1 == null || user2 == null)
                    {
                        // Handle case where users are not found
                        return StatusCode(404, "One or both users not found.");
                    }

                    // Initiate a battle between the two players
                    var result = await _battleService.RequestBattle(user1.UserId, user2.UserId);

                    // Return the battle result
                    return Ok(result);
                }
                else
                {
                    // Return a message indicating that both user IDs are not available yet
                    return StatusCode(409, "Both user IDs are not available yet. Please try again.");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("begin")]
        [Authorize] // Requires authorization to access this endpoint
        public async Task<IActionResult> BeginBattleAsync()
        {
            try
            {
                // Extract user IDs from JWT tokens of both API requests
                string user1Token = HttpContext.Request.Headers["Authorization"];
                string user2Token = HttpContext.Request.Headers["Authorization"];

                // Get user IDs from tokens
                var user1Id = _jwtAuthenticationService.GetUsernameFromToken(user1Token);
                var user2Id = _jwtAuthenticationService.GetUsernameFromToken(user2Token);

                // Get users from database
                var user1 = await _context.Users.FirstOrDefaultAsync(u => u.Username == user1Id);
                var user2 = await _context.Users.FirstOrDefaultAsync(u => u.Username == user2Id);

                // Check if both users are found
                if (user1 == null || user2 == null)
                {
                    // Handle case where users are not found
                    return StatusCode(404, "One or both users not found.");
                }

                // Initiate a battle between the two players
                var result = await _battleService.RequestBattle(user1.UserId, user2.UserId);

                // Return the battle result
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
