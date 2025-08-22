using ChatService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatService.Services
{
    public interface IConversationService
    {
        Task<Conversations?> GetConversationByIdAsync(Guid id);
        Task<IEnumerable<Conversations>> GetUserConversationsAsync(Guid userId);
        Task<Conversations> CreateConversationAsync(Conversations conversation, Guid adminId, List<Guid> participantIds);
        Task UpdateConversationAsync(Conversations conversation);
        Task DeleteConversationAsync(Guid id);
    }
}
