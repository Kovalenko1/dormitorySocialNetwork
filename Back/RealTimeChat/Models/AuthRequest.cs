using System.ComponentModel.DataAnnotations;

namespace RealTimeChat.Models
{
    public class AuthRequest
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}
