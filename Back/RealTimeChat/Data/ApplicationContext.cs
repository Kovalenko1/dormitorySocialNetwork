using Microsoft.EntityFrameworkCore;
using RealTimeChat.Models;

namespace RealTimeChat.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> users { get; set; } = null!;
        public DbSet<UserConnection> UserConnections { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) 
        {
            Database.EnsureCreated();
        }
    }
}
