using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealTimeChat.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
