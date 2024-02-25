using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace mtcg
{
    public class JwtAuthenticationService
    {
        private readonly IConfiguration _configuration;

        public JwtAuthenticationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateJwtToken(string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, username)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public bool IsAuthorized(string username, string token)
        {
            // Verify the JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);

            try
            {
                // Validate the token
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                }, out SecurityToken validatedToken);

                // Extract the claims from the validated token
                var jwtToken = (JwtSecurityToken)validatedToken;
                var claims = jwtToken.Claims;
                var tokenUsername = claims.First(claim => claim.Type == ClaimTypes.Name).Value;

                // Check if the username in the token matches the provided username
                return tokenUsername == username || IsAdmin(username); // Add IsAdmin check if needed
            }
            catch (Exception)
            {
                // Token validation failed, user is not authorized
                return false;
            }
        }

        public string GetUsernameFromToken(string authorizationHeader)
        {
            var token = authorizationHeader?.Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return null; // No token found in the authorization header
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);

            try
            {
                // Validate the token
                var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                }, out _);

                // Extract the username claim from the validated token
                var usernameClaim = claimsPrincipal.FindFirst(ClaimTypes.Name);

                if (usernameClaim == null)
                {
                    return null; // No username claim found in the token
                }

                return usernameClaim.Value; // Return the username extracted from the token
            }
            catch (Exception)
            {
                return null; // Token validation failed
            }
        }

        private bool IsAdmin(string username)
        {
            // Implement logic to check if the user is an admin (e.g., based on the username)
            return username == "admin"; // Replace with your actual logic
        }

        // Method to generate an admin JWT token
        private string GenerateAdminJwtToken(string adminUsername)
        {
            // Implement your logic to generate an admin token with appropriate claims
            // You can use the JwtSecurityTokenHandler to create the token
            // Example:
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim(ClaimTypes.Name, adminUsername),
            new Claim("IsAdmin", "true") // Custom claim indicating admin status
                                         // You can add other necessary claims here
                }),
                Expires = DateTime.UtcNow.AddHours(2), // Set expiration time (e.g., 2 hours from now)
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


    }
}

