using RealTimeChat.Models;

namespace RealTimeChat.Services
{
    public interface IChatService
    {
        Task<List<Chat>> GetUserChatsAsync(int userId);
    }
}