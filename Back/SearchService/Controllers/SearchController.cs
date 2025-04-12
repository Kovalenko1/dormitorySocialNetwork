using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SearchService.Data;

namespace RealTimeChat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public SearchController(ApplicationContext context) 
        {
            _context = context;
        }

        [HttpGet("usersSearch")]
        public async Task<IActionResult> SearchUsers(string query)
        {
            var users = await _context.Users
                .Where(u => u.Username.Contains(query))
                .ToListAsync();

            return Ok(users);
        }
    }
}
