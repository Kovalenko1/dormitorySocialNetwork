using RealTimeChat.Models;

namespace RealTimeChat.Services
{
    public interface IChatService
    {
        Task<List<PrivateChat>> GetUserChatsAsync(int userId);
    }
}