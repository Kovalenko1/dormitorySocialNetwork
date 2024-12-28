namespace NotificationService;

public class RabbitMqOptions
{
    public string HostName { get; set; } = "localhost";
    public string UserName { get; set; } = "guest";
    public string Password { get; set; } = "guest";
    public string Exchange { get; set; } = "ChatEventsExchange";
    public string Queue { get; set; } = "NewMessageQueue";
    public string RoutingKey { get; set; } = "chat.newmessage";
}