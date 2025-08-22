using ChatService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatService.Services
{
    public interface IMessageService
    {
        Task<Messages?> GetMessageByIdAsync(Guid id);
        Task<IEnumerable<Messages>> GetMessageByRoomIdAsync(Guid conversationId, int? take = null, DateTime? before = null);
        Task<Messages>CreateMessageAsync(Messages message);
        Task UpdateMessageAsync(Messages message);
        Task DeleteMessageAsync(Guid id);
    }
}
