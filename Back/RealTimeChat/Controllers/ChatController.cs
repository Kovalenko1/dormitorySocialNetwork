using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RealTimeChat.Data;

namespace RealTimeChat.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly ApplicationContext _context;
        public ChatController(ApplicationContext context)
        {
            _context = context;
        }
        
        [HttpGet("api/chats/user/{userId}")]
        public async Task<IActionResult> GetUserChats(int userId)
        {
            var userChats = await _context.PrivateChats
                .Where(pc => pc.User1Id == userId || pc.User2Id == userId)
                .ToListAsync();

            if (userChats == null)
            {
                return NotFound();
            }

            return Ok(userChats);
        }

    }

}
