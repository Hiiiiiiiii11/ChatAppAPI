
using ChatRepository.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRepository.Data
{
    public class ChatDbContext : DbContext
    {
        public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options) { }

        public DbSet<Conversations> Conversations { get; set; }
        public DbSet<Participants> Participants { get; set; }
        public DbSet<Messages> Messages { get; set; }
        public DbSet<MessageDeletion> MessageDeletions { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseLazyLoadingProxies(); 
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Messages
            modelBuilder.Entity<Messages>(entity =>
            {
                entity.HasKey(m => m.Id);

                entity.Property(m => m.Content)
                      .IsRequired()
                      .HasMaxLength(2000);

                entity.HasMany(m => m.MessageDeletions)
                      .WithOne(md => md.Message)
                      .HasForeignKey(md => md.MessageId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // MessageDeletion
            modelBuilder.Entity<MessageDeletion>(entity =>
            {
                entity.HasKey(md => md.Id);

                entity.HasIndex(md => new { md.MessageId, md.UserId })
                      .IsUnique(); // đảm bảo 1 user chỉ xóa 1 lần với 1 message

                entity.Property(md => md.DeletedAt)
                      .IsRequired();
            });
        }
    }
}
//dotnet ef migrations add addChatDb --project ConversationService --startup-project ChatAppAPI --context ChatDbContext
// dotnet ef database update --project ConversationService --startup-project ChatAppAPI --context ChatDbContext