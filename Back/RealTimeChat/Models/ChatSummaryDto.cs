namespace RealTimeChat.Models;

public class ChatSummaryDto
{
    public int ChatId { get; set; }
    public string OtherUserName { get; set; }        
    public string LastMessageText { get; set; }       
    public DateTime? LastMessageTime { get; set; } 
}