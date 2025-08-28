
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
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseLazyLoadingProxies(); 
            }
        }
    }
}
//dotnet ef migrations add addChatDb --project ConversationService --startup-project ChatAppAPI --context ChatDbContext
// dotnet ef database update --project ConversationService --startup-project ChatAppAPI --context ChatDbContext