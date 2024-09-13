using System.ComponentModel.DataAnnotations;

namespace RealTimeChat.Models
{
    public class PrivateChat
    {
        [Key]
        public int Id { get; set; }
        public string ChatRoomName { get; set; }
        public int User1Id { get; set; }
        public int User2Id { get; set; }
        public User User1 { get; set; } 
        public User User2 { get; set; }
    }


}
