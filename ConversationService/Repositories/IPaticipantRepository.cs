using ChatService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatService.Repositories
{
    public interface IPaticipantRepository
    {
        Task<IEnumerable<Participants>> GetParticipantsByConversationIdAsync(Guid conversationId);
        Task<Participants?> GetParticipantAsync(Guid conversationId, Guid userId);
        Task AddParticipantAsync(Participants participant);
        Task RemoveParticipantAsync(Guid conversationId, Guid userId);
        Task SaveChangesAsync();

    }
}
