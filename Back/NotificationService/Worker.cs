using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace NotificationService;

public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly RabbitMqOptions _rabbitOptions;
        private IConnection? _connection;
        private IModel? _channel;

        public Worker(ILogger<Worker> logger, IOptions<RabbitMqOptions> rabbitOptions)
        {
            _logger = logger;
            _rabbitOptions = rabbitOptions.Value;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Notification Worker starting...");

            var factory = new ConnectionFactory
            {
                HostName = _rabbitOptions.HostName,
                UserName = _rabbitOptions.UserName,
                Password = _rabbitOptions.Password
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(
                exchange: _rabbitOptions.Exchange, 
                type: "direct", 
                durable: true,
                autoDelete: false);

            _channel.QueueDeclare(
                queue: _rabbitOptions.Queue, 
                durable: true, 
                exclusive: false, 
                autoDelete: false);

            _channel.QueueBind(
                queue: _rabbitOptions.Queue,
                exchange: _rabbitOptions.Exchange,
                routingKey: _rabbitOptions.RoutingKey);

            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (ch, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var messageStr = Encoding.UTF8.GetString(body);

                    _logger.LogInformation("Received message: {msg}", messageStr);

                    var msg = JsonSerializer.Deserialize<NewMessageEvent>(messageStr);

                    if (msg != null)
                    {
                        await SendNotification(msg);
                    }

                    _channel.BasicAck(ea.DeliveryTag, multiple: false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing message");
                }
            };

            _channel.BasicConsume(
                queue: _rabbitOptions.Queue, 
                autoAck: false,
                consumer: consumer);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }

        private Task SendNotification(NewMessageEvent msg)
        {
            // Рано или поздно тут появится логика отправки уведомления пользователей, а пока просто логирование...
            // TODO: Добавить логику отправки уведомления пользователю
            _logger.LogInformation(
                "Sending notification to user {ReceiverId} about new message {MessageId} from {SenderId}", 
                msg.ReceiverId, 
                msg.MessageId, 
                msg.SenderId);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
            base.Dispose();
        }
    }

    public class NewMessageEvent
    {
        public int MessageId { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string? Text { get; set; }
        public DateTime Timestamp { get; set; }
    }
