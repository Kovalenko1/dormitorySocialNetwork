using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealTimeChat.Models;

[Table("userconnections")]
public class UserConnection
{
    [Key]
    public string connectionId { get; set; }
    public string userName { get; set; }
    public string chatRoom { get; set; }
}