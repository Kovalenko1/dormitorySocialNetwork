using System.ComponentModel.DataAnnotations;

namespace RealTimeChat.Models
{
    public class AuthResponse
    {
        public required string Token { get; set; }
        public required string Username { get; set; }
        public required string Role { get; set; }
    }
}
