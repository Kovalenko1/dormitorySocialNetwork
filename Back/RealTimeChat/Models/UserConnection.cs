using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealTimeChat.Models;

[Table("userconnections")]
public class UserConnection
{
    [Key]
    public required string ConnectionId { get; set; }
    public required string Username { get; set; }
    public required string ChatRoom { get; set; }
}