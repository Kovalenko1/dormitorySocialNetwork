using Microsoft.EntityFrameworkCore;
using SearchService.Models;

namespace SearchService.Data;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
}