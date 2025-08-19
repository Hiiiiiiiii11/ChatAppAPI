using ChatService.Data;
using ChatService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatService.Repositories
{
    public class ConversationRepository: IConversationRepository
    {
        private readonly ChatDbContext _context;
        public ConversationRepository(ChatDbContext context)
        {
            _context = context;
        }

        public async Task AddConversationAsync(Conversations conversation)
        {
            await _context.Conversations.AddAsync(conversation);
        }

        public Task DeleteConversationAsync(Guid id)
        {
            var conversation = _context.Conversations
                .FirstOrDefaultAsync(c => c.Id == id);
            if (conversation != null)
            {
                _context.Conversations.Remove(conversation.Result);
                return _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("Conversation not found.");
            }
        }

        public Task<Conversations?> GetConversationByIdAsync(Guid id)
        {
            return _context.Conversations.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Conversations>> GetUserConversationsAsync(Guid userId)
        {
            var conversation =  await _context.Conversations.Where(c=>c.Participants.Any(p => p.UserId == userId)).ToListAsync();
            return conversation;
        }

        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public Task UpdateConversationAsync(Conversations conversation)
        {
            _context.Conversations.Update(conversation);
            return _context.SaveChangesAsync();
        }
    }
}
