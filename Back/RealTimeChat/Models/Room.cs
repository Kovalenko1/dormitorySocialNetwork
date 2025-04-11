using System.ComponentModel.DataAnnotations;

namespace RealTimeChat.Models;

public class Room
{
    [Key]
    public int Id { get; set; }

    public int DormitoryId { get; set; }
    public Dormitory Dormitory { get; set; }

    public int Floor { get; set; }

    [MaxLength(10)]
    public string RoomNumber { get; set; }

    public ICollection<User> Users { get; set; }
}