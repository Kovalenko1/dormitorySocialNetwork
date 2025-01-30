using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealTimeChat.Models
{
    public class PrivateChat
    {
        [Key]
        public int Id { get; set; }
        public string RoomName { get; set; }
        public int User1Id { get; set; }
        public int User2Id { get; set; }
        
        [ForeignKey(nameof(User1Id))]
        public User User1 { get; set; } 
        
        [ForeignKey(nameof(User2Id))]
        public User User2 { get; set; }
        
        public ICollection<Message> Messages { get; set; }
    }
}
