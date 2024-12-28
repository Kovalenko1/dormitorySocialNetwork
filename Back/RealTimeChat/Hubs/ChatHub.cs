using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using RealTimeChat.Data;
using RealTimeChat.Models;
using RealTimeChat.Services;

namespace RealTimeChat.Hubs
{
    public interface IChatClient
    {
        Task ReceiveMessage(string userName, Message message);
        Task UserJoined(string userName);
        Task UserLeft(string userName);
    }

    public class ChatHub : Hub<IChatClient>
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<ChatHub> _logger;
        private readonly IRabbitMqService _rabbitMqService;

        public ChatHub(ApplicationContext context, ILogger<ChatHub> logger, IRabbitMqService rabbitMqService)
        {
            _context = context;
            _logger = logger;
            _rabbitMqService = rabbitMqService;
        }


        public async Task<PrivateChat> GetOrCreatePrivateChat(int userId1, int userId2)
        {
            var chatRoomName = GenerateChatRoomName(userId1, userId2);

            var privateChat = await _context.PrivateChats
                .FirstOrDefaultAsync(pc => pc.ChatRoomName == chatRoomName);

            if (privateChat == null)
            {
                privateChat = new PrivateChat
                {
                    User1Id = userId1,
                    User2Id = userId2,
                    ChatRoomName = chatRoomName
                };
                _context.PrivateChats.Add(privateChat);
                await _context.SaveChangesAsync();
            }


            return privateChat;
        }

        private string GenerateChatRoomName(int userId1, int userId2)
        {
            return string.CompareOrdinal(userId1.ToString(), userId2.ToString()) < 0
                ? $"{userId1}-{userId2}"
                : $"{userId2}-{userId1}";
        }

        public async Task JoinPrivateChat(int userId1, int userId2)
        {
            try
            {
                _logger.LogInformation("JoinPrivateChat called with userId1: {UserId1}, userId2: {UserId2}", userId1,
                    userId2);

                var privateChat = await GetOrCreatePrivateChat(userId1, userId2);

                await Groups.AddToGroupAsync(Context.ConnectionId, privateChat.ChatRoomName);

                var user = await _context.Users.FindAsync(userId1);
                if (user == null)
                {
                    _logger.LogWarning("User not found with ID: {UserId}", userId1);
                    throw new Exception($"User with ID {userId1} not found");
                }

                var userConnection = new UserConnection
                {
                    ConnectionId = Context.ConnectionId,
                    UserId = user.Id,
                    ChatRoom = privateChat.ChatRoomName
                };
                _context.UserConnections.Add(userConnection);
                await _context.SaveChangesAsync();

                await Clients.Group(privateChat.ChatRoomName).UserJoined(user.Username);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in JoinPrivateChat");
                throw;
            }
        }


        public async Task SendMessageToPrivateChat(int userId1, int userId2, Message userMessage)
        {
            var privateChat = await GetOrCreatePrivateChat(userId1, userId2);

            var connection = await _context.UserConnections
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.ConnectionId == Context.ConnectionId);

            if (connection != null)
            {
                var message = new Message
                {
                    SenderId = userId1,
                    Text = userMessage.Text,
                    File = userMessage.File,
                    Timestamp = DateTime.UtcNow,
                    PrivateChatId = privateChat.Id
                };
        
                _context.Messages.Add(message);
                await _context.SaveChangesAsync();

                await Clients.Group(privateChat.ChatRoomName)
                    .ReceiveMessage(connection.User.Username, message);

                var newMessageEvent = new
                {
                    MessageId = message.Id,
                    SenderId = userId1,
                    ReceiverId = (userId1 == privateChat.User1Id) 
                        ? privateChat.User2Id 
                        : privateChat.User1Id,
                    Text = userMessage.Text,
                    Timestamp = message.Timestamp
                };
                _rabbitMqService.PublishMessage("chat.newmessage", newMessageEvent);
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var connection = await _context.UserConnections
                .FirstOrDefaultAsync(c => c.ConnectionId == Context.ConnectionId);

            if (connection != null)
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, connection.ChatRoom);
                await Clients.Group(connection.ChatRoom).UserLeft(connection.User.Username);

                _context.UserConnections.Remove(connection);
                await _context.SaveChangesAsync();
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
