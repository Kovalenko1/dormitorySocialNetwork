using System.ComponentModel.DataAnnotations;

namespace RealTimeChat.Models
{
    public class GroupChat
    {
        [Key]
        public int Id { get; set; }
        public string GroupName { get; set; }
        public List<GroupChatParticipant> Participants { get; set; } = new List<GroupChatParticipant>();
    }

}
