using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace RealTimeChat.Services;

public class RabbitMqService : IRabbitMqService, IDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly string _exchangeName;

    public RabbitMqService(IOptions<RabbitMqOptions> config)
    {
        var settings = config.Value;
        _exchangeName = settings.Exchange;

        var factory = new ConnectionFactory
        {
            HostName = settings.HostName,
            UserName = settings.UserName,
            Password = settings.Password
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare(
            exchange: _exchangeName, 
            type: "direct", 
            durable: true, 
            autoDelete: false);
    }

    public void PublishMessage(string routingKey, object message)
    {
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
        _channel.BasicPublish(
            exchange: _exchangeName,
            routingKey: routingKey,
            basicProperties: null,
            body: body
        );
    }

    public void Dispose()
    {
        if (_channel.IsOpen) _channel.Close();
        _channel.Dispose();
        _connection.Close();
        _connection.Dispose();
    }
}

public class RabbitMqOptions
{
    public string HostName { get; set; } = "localhost";
    public string UserName { get; set; } = "guest";
    public string Password { get; set; } = "guest";
    public string Exchange { get; set; } = "ChatEventsExchange";
    public string Queue { get; set; } = "NewMessageQueue";
    public string RoutingKey { get; set; } = "chat.newmessage";
}


