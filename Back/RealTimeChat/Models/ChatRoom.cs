using System.ComponentModel.DataAnnotations;

namespace RealTimeChat.Models
{
    public class ChatRoom
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }
        public List<UserConnection> Participants { get; set; } = new List<UserConnection>();
    }

}
