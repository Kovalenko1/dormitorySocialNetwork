using System.ComponentModel.DataAnnotations;

namespace RealTimeChat.Models
{
    public class GroupChatParticipant
    {
        [Key]
        public int Id { get; set; }
        public int GroupChatId { get; set; }
        public GroupChat GroupChat { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }

}
