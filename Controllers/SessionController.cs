using Microsoft.AspNetCore.Mvc;
using mtcg;

[ApiController]
[Route("sessions")]
public class SessionsController : ControllerBase
{
    private readonly mtcgDbContext _context;
    private readonly JwtAuthenticationService _jwtAuthenticationService;

    public SessionsController(mtcgDbContext context, JwtAuthenticationService jwtAuthenticationService)
    {
        _context = context;
        _jwtAuthenticationService = jwtAuthenticationService;
    }

    [HttpPost]
    public IActionResult Login([FromBody] UserCredentials credentials)
    {
        // Authenticate user based on credentials
        var user = _context.Users.FirstOrDefault(u => u.Username == credentials.Username && u.Password == credentials.Password);

        if (user == null)
        {
            // User authentication failed
            return Unauthorized(new { Message = "Invalid username/password provided" });
        }

        // Generate JWT token for authenticated user
        var token = _jwtAuthenticationService.GenerateJwtToken(credentials.Username);

        // Return the generated token in the response
        return Ok(new { Token = token });
    }
}
