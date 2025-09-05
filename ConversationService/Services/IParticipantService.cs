
using ChatRepository.Model.Request;
using ChatRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatService.Services
{
    public interface IParticipantService
    {
        Task<IEnumerable<Participants>> GetParticipantsByConversationIdAsync(Guid conversationId);
        Task<Participants?> GetParticipantAsync(Guid conversationId, Guid userId);
        Task<List<Participants>>AddParticipantToConversation(Guid conversationId, List<Guid> userIds);
        Task<IEnumerable<Participants>> GetBannedParticipantsByConversationIdAsync(Guid conversationId);
        Task<IEnumerable<Participants>> GetBanChatParticipantsByConversationIdAsync(Guid conversationId);
        Task<IEnumerable<Participants>> RemoveParticipantsAsync(Guid conversationId, List<Guid> userIds);
        Task<IEnumerable<Participants>> BannedParticipantsAsync(Guid conversationId, List<Guid> userIds);
        Task<IEnumerable<Participants>> UnBannedParticipantsAsync(Guid conversationId, List<Guid> userIds);
        Task<IEnumerable<Participants>> BanChatParticipantsAsync(Guid conversationId, List<Guid> userIds);
        Task<IEnumerable<Participants>> UnBanChatParticipantsAsync(Guid conversationId, List<Guid> userIds);
    }
}
