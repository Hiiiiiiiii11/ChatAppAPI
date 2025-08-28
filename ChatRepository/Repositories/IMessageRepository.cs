using ChatRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRepository.Repositories
{
    public interface IMessageRepository
    {
        Task<IEnumerable<Messages>> GetMessagesByRoomIdAsync(Guid conversationId, int? take = null, DateTime? before = null);
        Task<Messages?> GetMessageByIdAsync(Guid id);
        Task AddMessageAsync(Messages message);
        Task UpdateMessageAsync(Messages message);
        Task DeleteMessageAsync(Guid id);
        Task SaveChangesAsync();

    }
}
