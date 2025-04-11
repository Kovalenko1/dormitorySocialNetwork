using Microsoft.EntityFrameworkCore;
using RealTimeChat.Data;
using RealTimeChat.Models;

namespace RealTimeChat.Services
{
    public class ChatService : IChatService
    {
        private readonly ApplicationContext _context;

        public ChatService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<List<Chat>> GetUserChatsAsync(int userId)
        {
            return await _context.PrivateChats
                .Where(pc => pc.User1Id == userId || pc.User2Id == userId)
                .Include(pc => pc.User1)
                .Include(pc => pc.User2)
                .Include(pc => pc.Messages)
                .ToListAsync();
        }
    }
}