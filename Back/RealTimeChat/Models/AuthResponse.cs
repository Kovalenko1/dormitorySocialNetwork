using System.ComponentModel.DataAnnotations;

namespace RealTimeChat.Models
{
    public class AuthResponse
    {
        public string Token { get; set; }
        public string username { get; set; }
        public string role { get; set; }
    }
}
