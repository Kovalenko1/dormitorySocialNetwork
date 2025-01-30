using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using RealTimeChat.Data;
using RealTimeChat.Models;
using RealTimeChat.Services;

namespace RealTimeChat.Hubs
{
    public interface IChatClient
    {
        Task ReceiveMessage(string roomName, Message message);
        
        Task UpdateChatList(ChatSummaryDto chatSummary);

        Task UserJoinedChat(int chatId, string userName);
        Task UserLeftChat(int chatId, string userName);
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

        public override async Task OnConnectedAsync()
        {
            try
            {
                var httpContext = Context.GetHttpContext();
                if (httpContext.Request.Query.TryGetValue("userId", out var userIdStr))
                {
                    if (int.TryParse(userIdStr, out int userId))
                    {
                        await JoinAllUserChats(userId);
                        var userChats = await GetUserChats(userId);
                        foreach (var chat in userChats)
                        {
                            await Clients.Caller.UpdateChatList(chat);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in OnConnectedAsync");
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            try
            {
                var connection = await _context.UserConnections
                    .FirstOrDefaultAsync(c => c.ConnectionId == Context.ConnectionId);

                if (connection != null)
                {
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, connection.ChatRoom);

                    var user = await _context.Users.FindAsync(connection.UserId);
                    if (user != null)
                    {
                        var chat = await _context.PrivateChats
                            .FirstOrDefaultAsync(pc => pc.RoomName == connection.ChatRoom);

                        if (chat != null)
                        {
                            await Clients.Group(connection.ChatRoom)
                                .UserLeftChat(chat.Id, user.Username);
                        }
                    }

                    _context.UserConnections.Remove(connection);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error OnDisconnectedAsync");
            }

            await base.OnDisconnectedAsync(exception);
        }

        private async Task JoinAllUserChats(int userId)
        {
            var privateChats = await _context.PrivateChats
                .Where(pc => pc.User1Id == userId || pc.User2Id == userId)
                .ToListAsync();

            foreach (var chat in privateChats)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, chat.RoomName);

                var userConn = new UserConnection
                {
                    ConnectionId = Context.ConnectionId,
                    UserId = userId,
                    ChatRoom = chat.RoomName
                };
                _context.UserConnections.Add(userConn);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<ChatSummaryDto>> GetUserChats(int userId)
        {
            var privateChats = await _context.PrivateChats
                .Where(pc => pc.User1Id == userId || pc.User2Id == userId)
                .ToListAsync();

            var result = new List<ChatSummaryDto>();

            foreach (var chat in privateChats)
            {
                var otherUserId = (chat.User1Id == userId) ? chat.User2Id : chat.User1Id;
                var otherUser = await _context.Users.FindAsync(otherUserId);

                var lastMessage = await _context.Messages
                    .Where(m => m.PrivateChatId == chat.Id)
                    .OrderByDescending(m => m.Timestamp)
                    .FirstOrDefaultAsync();

                result.Add(new ChatSummaryDto
                {
                    ChatId = chat.Id,
                    OtherUserName = otherUser?.Username ?? "Unknown",
                    LastMessageText = lastMessage?.Text,
                    LastMessageTime = lastMessage?.Timestamp
                });
            }

            return result;
        }
        
        public async Task<List<Message>> GetChatMessages(int chatId, int userId)
        {
            var chat = await _context.PrivateChats.FindAsync(chatId);
            if (chat == null)
                throw new Exception("Chat not found");

            if (chat.User1Id != userId && chat.User2Id != userId)
                throw new Exception("User not a participant of this chat");

            var messages = await _context.Messages
                .Where(m => m.PrivateChatId == chat.Id)
                .OrderBy(m => m.Timestamp)
                .ToListAsync();

            return messages.Select(m => new Message
            {
                Id = m.Id,
                SenderId = m.SenderId,
                Text = m.Text,
                File = m.File,
                Timestamp = m.Timestamp
            }).ToList();
        }

        public async Task SendMessage(Message userMessage)
        {
            var chat = await CreateOrGetChatWithUser(userMessage.SenderId, userMessage.ReceiverId);
            
            var message = new Message
            {
                ChatName = chat.RoomName,
                PrivateChatId = chat.Id,
                SenderId = userMessage.SenderId,
                ReceiverId = userMessage.ReceiverId,
                Text = userMessage.Text,
                File = userMessage.File,
                Timestamp = DateTime.UtcNow
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            await Clients.Group(chat.RoomName)
                .ReceiveMessage(chat.RoomName, message);

            await UpdateChatListForParticipants(chat, message);

            var receiverId = (chat.User1Id == userMessage.SenderId)
                ? chat.User2Id
                : chat.User1Id;

            var newMessageEvent = new
            {
                MessageId = message.Id,
                SenderId = userMessage.SenderId,
                ReceiverId = receiverId,
                Text = message.Text,
                Timestamp = message.Timestamp
            };
            _rabbitMqService.PublishMessage("chat.newmessage", newMessageEvent);
        }

        private async Task UpdateChatListForParticipants(PrivateChat chat, Message lastMessage)
        {
            await UpdateSingleParticipant(chat, lastMessage, chat.User1Id);
            await UpdateSingleParticipant(chat, lastMessage, chat.User2Id);
        }

        private async Task UpdateSingleParticipant(PrivateChat chat, Message lastMessage, int participantId)
        {
            var otherUserId = (chat.User1Id == participantId) ? chat.User2Id : chat.User1Id;
            var otherUser = await _context.Users.FindAsync(otherUserId);

            var summary = new ChatSummaryDto
            {
                ChatId = chat.Id,
                OtherUserName = otherUser?.Username ?? "Unknown",
                LastMessageText = lastMessage.Text,
                LastMessageTime = lastMessage.Timestamp
            };

            await Clients.Group(chat.RoomName)
                .UpdateChatList(summary);
        }

        public async Task<PrivateChat> CreateOrGetChatWithUser(int currentUserId, int otherUserId)
        {
            var chatRoomName = GenerateChatRoomName(currentUserId, otherUserId);

            var privateChat = await _context.PrivateChats
                .FirstOrDefaultAsync(pc => pc.RoomName == chatRoomName);

            if (privateChat == null)
            {
                var user1 = await _context.Users.FindAsync(currentUserId);
                var user2 = await _context.Users.FindAsync(otherUserId);

                if (user1 == null || user2 == null)
                {
                    throw new Exception("Один из пользователей не существует.");
                }

                privateChat = new PrivateChat
                {
                    User1Id = currentUserId,
                    User2Id = otherUserId,
                    RoomName = chatRoomName
                };
                _context.PrivateChats.Add(privateChat);
                await _context.SaveChangesAsync();
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, chatRoomName);
            
            var existingConnection = await _context.UserConnections
                .FirstOrDefaultAsync(uc => uc.ConnectionId == Context.ConnectionId && uc.ChatRoom == chatRoomName);

            if (existingConnection == null)
            {
                var userConn = new UserConnection
                {
                    ConnectionId = Context.ConnectionId,
                    UserId = currentUserId,
                    ChatRoom = chatRoomName
                };
                _context.UserConnections.Add(userConn);
                await _context.SaveChangesAsync();
            }

            var otherUser = await _context.Users.FindAsync(otherUserId);
            var summary = new ChatSummaryDto
            {
                ChatId = privateChat.Id,
                OtherUserName = otherUser?.Username ?? "Unknown",
                LastMessageText = null,
                LastMessageTime = null
            };

            await Clients.Group(chatRoomName).UpdateChatList(summary);

            return privateChat;
        }

        private string GenerateChatRoomName(int userId1, int userId2)
        {
            return (userId1 < userId2) 
                ? $"{userId1}-{userId2}" 
                : $"{userId2}-{userId1}";
        }
    }
}
