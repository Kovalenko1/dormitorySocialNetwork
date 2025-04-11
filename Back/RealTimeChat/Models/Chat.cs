using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealTimeChat.Models
{
    public class Chat
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string ChatName { get; set; }
        
        [Required]
        public ChatType Type { get; set; }
        public int User1Id { get; set; }
        public int User2Id { get; set; }
        
        [ForeignKey(nameof(User1Id))]
        public User User1 { get; set; } 
        
        [ForeignKey(nameof(User2Id))]
        public User User2 { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<Message> Messages { get; set; }
    }
}
