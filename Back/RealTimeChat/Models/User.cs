using System.ComponentModel.DataAnnotations;

namespace RealTimeChat.Models
{
    public class User
    {
        [Key]
        public required int Id { get; set; }
        public required string Username { get; set; }  
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Bio { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Role { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
