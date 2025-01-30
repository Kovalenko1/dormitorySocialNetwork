using Microsoft.EntityFrameworkCore;
using RealTimeChat.Models;

namespace RealTimeChat.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<PrivateChat> PrivateChats { get; set; } = null!;
        public DbSet<UserConnection> UserConnections { get; set; } = null!;
        public DbSet<Message> Messages { get; set; } = null!;

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PrivateChat>()
                .HasOne(pc => pc.User1)
                .WithMany()
                .HasForeignKey(pc => pc.User1Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PrivateChat>()
                .HasOne(pc => pc.User2)
                .WithMany()
                .HasForeignKey(pc => pc.User2Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.PrivateChat)
                .WithMany(pc => pc.Messages)
                .HasForeignKey(m => m.PrivateChatId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserConnection>()
                .HasOne(uc => uc.User)
                .WithMany(u => u.Connections)
                .HasForeignKey(uc => uc.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}