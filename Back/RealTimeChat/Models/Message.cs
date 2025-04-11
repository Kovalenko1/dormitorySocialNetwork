using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RealTimeChat.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        public string ChatName { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string Text { get; set; }
        
        [Column(TypeName = "text[]")]
        public string[] File { get; set; } = System.Array.Empty<string>();
        public DateTime Timestamp { get; set; }
        
        [JsonIgnore]
        public User Sender { get; set; }
        public int ChatId { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(Id))]
        public Chat Chat { get; set; }
    }
}
