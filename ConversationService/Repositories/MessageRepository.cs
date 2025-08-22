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
    public class MessageRepository : IMessageRepository
    {
        private readonly ChatDbContext _context;
        public MessageRepository(ChatDbContext context)
        {
            _context = context;
        }
        public async Task AddMessageAsync(Messages message)
        {
            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMessageAsync(Guid id)
        {
            var message = await _context.Messages.FirstOrDefaultAsync(m => m.Id == id);
            if (message == null)
                throw new KeyNotFoundException("Message not found.");

            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();
        }

        public Task<Messages?> GetMessageByIdAsync(Guid id)
        {
            return _context.Messages
                 .Include(m => m.Conversation)
                 .AsNoTracking()
                 .FirstOrDefaultAsync(m => m.Id == id);
        }


        /// <summary>
        /// Lấy tin nhắn theo ConversationId, sắp xếp mới nhất trước.
        /// - before: lazy-load phân trang ngược (lấy các message cũ hơn thời điểm này)
        /// - take: số lượng cần lấy (ví dụ 20)
        /// </summary>
        public async Task<IEnumerable<Messages>> GetMessagesByRoomIdAsync(Guid conversationId, int? take = null, DateTime? before = null)
        {
            var query = _context.Messages.AsQueryable();
            query = query.Where(m => m.ConversationId == conversationId);
            if(before.HasValue)
                query = query.Where(m => m.SentAt < before.Value);
                query = query.OrderByDescending(m => m.SentAt);

            if(take.HasValue && take > 0)
                query = query.Take(take.Value);

            return await query.AsNoTracking().ToListAsync();
        }



        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public Task UpdateMessageAsync(Messages message)
        {
            _context.Messages.Update(message);
            return _context.SaveChangesAsync();
        }
    }
}
