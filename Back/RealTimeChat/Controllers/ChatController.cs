using Microsoft.AspNetCore.Mvc;
using RealTimeChat.Services;

namespace RealTimeChat.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpGet("api/chats/user/{userId}")]
        public async Task<IActionResult> GetUserChats(int userId)
        {
            var userChats = await _chatService.GetUserChatsAsync(userId);
            
            if (userChats == null || userChats.Count == 0)
            {
                return NotFound("No chats found for this user");
            }

            return Ok(userChats);
        }
    }
}