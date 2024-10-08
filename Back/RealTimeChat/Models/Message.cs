using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RealTimeChat.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        public int SenderId { get; set; }
        public string Text { get; set; }
        public List<string> File { get; set; }
        public DateTime Timestamp { get; set; }
        public int PrivateChatId { get; set; }

        [JsonIgnore]
        public User Sender { get; set; }
        
        [JsonIgnore]
        public PrivateChat PrivateChat { get; set; }
    }
}
