using ChatService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatService.Repositories
{
    public interface IChatRepository
    {
        Task<IEnumerable<Messages>> GetMessagesByRoomIdAsync(Guid ConversationId);
        Task<Messages?> GetMessageByIdAsync(Guid id);
        Task AddMessageAsync(Messages message);
        Task UpdateMessageAsync(Messages message);
        Task DeleteMessageAsync(Guid id);

    }
}
