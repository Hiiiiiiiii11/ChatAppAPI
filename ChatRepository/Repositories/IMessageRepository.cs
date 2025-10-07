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
        Task SendMessageAsync(Messages message);
        Task EditMessageAsync(Messages message);
        Task DeleteMessageWithAllAsync(Messages message);
        Task DeleteMessageOnlyUserAsync(MessageDeletion deletion);
        Task DeleteMessageAsync(Guid id);
        Task<IEnumerable<Messages>> SearchMessageAsync(Guid conversationId, string keyword, int? take = null, DateTime? before = null);
        Task<IEnumerable<MessageDeletion>> GetDeletionsByUserAsync(Guid userId);
        Task SaveChangesAsync();

    }
}
