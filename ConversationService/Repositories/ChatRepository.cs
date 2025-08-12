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
    public class ChatRepository : IChatRepository
    {
        private readonly ChatDbContext _context;
        public ChatRepository(ChatDbContext context)
        {
            _context = context;
        }

        public async Task AddMessageAsync(Messages message)
        {
            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();
        }

        public Task DeleteMessageAsync(Guid id)
        {
            var message = _context.Messages.FirstOrDefault(m => m.Id == id);
            if (message != null)
            {
                _context.Messages.Remove(message);
                return _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"Message with ID {id} not found.");
            }
        }

        public Task<Messages?> GetMessageByIdAsync(Guid id)
        {
            return _context.Messages
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Messages>> GetMessagesByRoomIdAsync(Guid ConversationId)
        {
           return await _context.Messages
                .Where(m => m.ConversationId == ConversationId)
                .ToListAsync();
        }

        public async Task UpdateMessageAsync(Messages message)
        {
            _context.Messages.Update(message);
            await _context.SaveChangesAsync();
        }
    }
}
