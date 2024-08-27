using System.ComponentModel.DataAnnotations;

namespace RealTimeChat.Models
{
    public class AuthRequest
    {
        public string email { get; set; }
        public string password { get; set; }
    }
}
