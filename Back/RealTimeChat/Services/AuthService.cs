using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RealTimeChat.Data;
using RealTimeChat.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RealTimeChat.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationContext _context;
        private readonly JwtSettings _jwtSettings;

        public AuthService(ApplicationContext context, IOptions<JwtSettings> jwtSettings)
        {
            _context = context;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<User> RegisterUserAsync(User newUser)
        {
            if (await _context.Users.AnyAsync(u => u.Username == newUser.Username))
            {
                throw new ArgumentException("User already exists");
            }

            var passwordHasher = new PasswordHasher<User>();
            newUser.Password = passwordHasher.HashPassword(newUser, newUser.Password);
            newUser.CreatedAt = DateTime.Now;
            
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return newUser;
        }

        public async Task<AuthResponse> LoginAsync(AuthRequest request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
                throw new UnauthorizedAccessException("Invalid username or password");

            var passwordHasher = new PasswordHasher<User>();
            var verifyResult = passwordHasher.VerifyHashedPassword(user, user.Password, request.Password);
            if (verifyResult == PasswordVerificationResult.Failed)
                throw new UnauthorizedAccessException("Invalid username or password");

            var userChats = await _context.PrivateChats
                .Where(pc => pc.User1Id == user.Id || pc.User2Id == user.Id)
                .Include(pc => pc.User1)
                .Include(pc => pc.User2)
                .Include(pc => pc.Messages)
                .ToListAsync();

            var token = GenerateJwtToken(user);

            return new AuthResponse
            {
                Id = user.Id,
                Token = token,
                Username = user.Username,
                Role = user.Role,
                Chats = userChats,
            };
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
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
