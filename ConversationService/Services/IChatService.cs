using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatService.Services
{
    public interface IChatService
    {
      Task<IEnumerable<Models.Messages>> GetMessagesByRoomIdAsync(Guid conversationId);
        Task<Models.Messages?> GetMessageByIdAsync(Guid id);
        Task AddMessageAsync(Models.Messages message);
        Task UpdateMessageAsync(Models.Messages message);
        Task DeleteMessageAsync(Guid id);
        Task<IEnumerable<Models.Messages>> SearchMessagesAsync(Guid conversationId, string searchTerm);
    }
}
