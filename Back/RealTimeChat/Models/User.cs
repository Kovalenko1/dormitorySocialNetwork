using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealTimeChat.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public required string Username { get; set; }
        
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        
        [Required]
        public required string Password { get; set; }
        public string Role { get; set; } = "user";
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Bio { get; set; }
        
        [Column(TypeName = "text[]")]
        public string[]? Photo { get; set; } = System.Array.Empty<string>();

        public DateTime CreatedAt { get; set; }
        public List<UserConnection>? Connections { get; set; }
        public List<PrivateChat>? PrivateChatsAsUser1 { get; set; }
        public List<PrivateChat>? PrivateChatsAsUser2 { get; set; }
    }
}
