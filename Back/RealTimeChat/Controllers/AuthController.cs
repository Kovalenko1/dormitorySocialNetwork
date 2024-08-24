using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RealTimeChat.Data;
using RealTimeChat.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;

namespace RealTimeChat.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly JwtSettings _jwtSettings;

        public AuthController(ApplicationContext context, IOptions<JwtSettings> jwtSettings)
        {
            _context = context;
            _jwtSettings = jwtSettings.Value;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User newUser)
        {
            if (_context.users.Any(u => u.username == newUser.username))
            {
                return BadRequest("User already exists");
            }

            _context.users.Add(newUser);
            await _context.SaveChangesAsync();
            return Ok(newUser);
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] AuthRequest request)
        {
            var user = _context.users.FirstOrDefault(u => u.username == request.username && u.password == request.password);

            if (user == null)
            {
                return Unauthorized("Invalid username or password");
            }

            var token = GenerateJwtToken(user);
            return Ok(new AuthResponse
            {
                Token = token,
                username = user.username,
                role = user.role,
            });
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
                new Claim(ClaimTypes.Role, user.role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtSettings.ExpiresInMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
