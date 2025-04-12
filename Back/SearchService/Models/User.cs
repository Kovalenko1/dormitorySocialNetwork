using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SearchService.Models
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
        public required string PasswordHash { get; set; }
        [Required]
        public string Role { get; set; } = "user";
        public int? Room { get; set; }
        public string? Phone { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Bio { get; set; }
        
        public int? DormitoryId { get; set; }
        public int? RoomId { get; set; }
        
        [Column(TypeName = "text[]")]
        public string[]? Photo { get; set; } = System.Array.Empty<string>();
    }
}
