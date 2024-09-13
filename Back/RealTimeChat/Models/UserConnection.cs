using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealTimeChat.Models;

[Table("UserConnections")]
public class UserConnection
{
    [Key]
    public int Id { get; set; }
    public string ConnectionId { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } 
    public string ChatRoom { get; set; }
}
