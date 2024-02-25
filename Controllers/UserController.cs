using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using mtcg;


[ApiController]
[Route("users")]
public class UserController : ControllerBase
{
    private readonly mtcgDbContext _context;
    private readonly JwtAuthenticationService _jwtAuthenticationService; // Add JwtAuthenticationService dependency

    public UserController(mtcgDbContext context, JwtAuthenticationService jwtAuthenticationService)
    {
        _context = context;
        _jwtAuthenticationService = jwtAuthenticationService; // Inject JwtAuthenticationService
    }

    [HttpPost]
    public IActionResult RegisterUser([FromBody] UserRegistrationRequest request)
    {
        // Validate the request data
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            // Check if a user with the same username already exists
            var existingUser = _context.Users.FirstOrDefault(u => u.Username == request.Username);

            if (existingUser != null)
            {
                // User with the same username already registered, return a conflict response
                return Conflict(new { Message = "User with the same username already registered" });
            }

            // Hash the user's password (use a secure hashing library)
            string hashedPassword = HashPassword(request.Password);

            // Create a new User object
            var user = new User
            {
                Username = request.Username,
                Password = hashedPassword,
                Coins = 20
                // You may set other user properties as needed
            };

            // Add the user to the database and save changes
            _context.Users.Add(user);
            _context.SaveChanges();

            // Generate JWT token for the registered user
            var token = _jwtAuthenticationService.GenerateJwtToken(request.Username);

            // Return the generated token along with the success response
            return Ok(new { Message = "User registered successfully" });
        }
        catch (Exception ex)
        {
            // Handle any exceptions (e.g., database errors) and return an error response
            return StatusCode(500, new { Message = "An error occurred while registering the user." });
        }
    }

    // Other methods...
    [HttpPut("{username}")]
    public IActionResult UpdateUserProfile(string username, [FromBody] UserProfile userProfile)
    {
        // Validate the request data
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            // Find the user in the database by username
            var existingUser = _context.Users.FirstOrDefault(u => u.Username == username);

            if (existingUser == null)
            {
                // User not found, return a 404 response
                return NotFound(new { Message = "User not found" });
            }

            // Update the user's profile with the new data
            existingUser.Username = userProfile.Name;

            // Check if the Bio property is provided in the userProfile
            if (userProfile.Bio != null)
            {
                existingUser.Bio = userProfile.Bio;
            }

            // Save changes to the database
            _context.SaveChanges();

            // Return a success response
            return Ok(new { Message = "User profile updated successfully" });
        }
        catch (Exception ex)
        {
            // Handle any exceptions (e.g., database errors) and return an error response
            return StatusCode(500, new { Message = "An error occurred while updating the user profile." });
        }
    }



    [HttpGet("{username}")]
    public IActionResult GetUserProfile(string username)
    {
        try
        {
            // Find the user in the database by username
            var user = _context.Users.FirstOrDefault(u => u.Username == username);

            if (user == null)
            {
                // User not found, return a 404 response
                return NotFound(new { Message = "User not found" });
            }

            // Map user data to UserProfile model
            var userProfile = new UserProfile
            {
                Id = user.UserId,
                Name = user.Username,
                // Assuming Bio and UserStats are properties of UserProfile
                
            };

            // Return user profile data
            return Ok(userProfile);
        }
        catch (Exception ex)
        {
            // Handle any exceptions (e.g., database errors) and return an error response
            return StatusCode(500, new { Message = "An error occurred while retrieving the user profile." });
        }
    }

    // Replace this with a secure password hashing method (e.g., bcrypt)
    public static string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder builder = new StringBuilder();

            foreach (byte b in hashedBytes)
            {
                builder.Append(b.ToString("x2")); // Convert each byte to a hexadecimal string
            }

            return builder.ToString();
        }
    }
}
