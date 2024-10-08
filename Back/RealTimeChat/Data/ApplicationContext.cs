using Microsoft.EntityFrameworkCore;
using RealTimeChat.Models;

namespace RealTimeChat.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<PrivateChat> PrivateChats { get; set; }
        //public DbSet<GroupChat> GroupChats { get; set; }
        //public DbSet<GroupChatParticipant> GroupChatParticipants { get; set; }
        public DbSet<UserConnection> UserConnections { get; set; }
        public DbSet<Message> Messages { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PrivateChat>()
                .HasOne(pc => pc.User1)
                .WithMany()
                .HasForeignKey(pc => pc.User1Id);

            modelBuilder.Entity<PrivateChat>()
                .HasOne(pc => pc.User2)
                .WithMany()
                .HasForeignKey(pc => pc.User2Id);
            
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany()
                .HasForeignKey(m => m.SenderId);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.PrivateChat)
                .WithMany(pc => pc.Messages)
                .HasForeignKey(m => m.PrivateChatId);
            

            // modelBuilder.Entity<GroupChatParticipant>()
            //     .HasKey(gcp => new { gcp.GroupChatId, gcp.UserId });
            //
            // modelBuilder.Entity<GroupChatParticipant>()
            //     .HasOne(gcp => gcp.GroupChat)
            //     .WithMany(gc => gc.Participants)
            //     .HasForeignKey(gcp => gcp.GroupChatId);
            //
            // modelBuilder.Entity<GroupChatParticipant>()
            //     .HasOne(gcp => gcp.User)
            //     .WithMany(u => u.GroupChatParticipants)
            //     .HasForeignKey(gcp => gcp.UserId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
