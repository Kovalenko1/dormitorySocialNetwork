using RealTimeChat.Models;

namespace RealTimeChat.Services
{
    public interface IAuthService
    {
        Task<User> RegisterUserAsync(User newUser);
        Task<AuthResponse> LoginAsync(AuthRequest request);
    }
}