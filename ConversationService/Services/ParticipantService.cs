


using ChatRepository.Model.Response;
using ChatRepository.Models;
using ChatRepository.Repositories;

namespace ChatService.Services
{
    public class ParticipantService : IParticipantService
    {
        private readonly IParticipantRepository _participantRepository;

        public ParticipantService(IParticipantRepository participantRepository)
        {
            _participantRepository = participantRepository;
            
        }

        public Task<IEnumerable<Participants>> BanChatParticipantsAsync(Guid conversationId, List<Guid> userIds)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Participants>> BannedParticipantsAsync(Guid conversationId, List<Guid> userIds)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Participants>> GetBannedParticipantsByConversationIdAsync(Guid conversationId)
        {
            throw new NotImplementedException();
        }

        public Task<Participants?> GetParticipantAsync(Guid conversationId, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Participants>> GetParticipantsByConversationIdAsync(Guid conversationId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Participants>> RemoveParticipantsAsync(Guid conversationId, List<Guid> userIds)
        {
            throw new NotImplementedException();
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Participants>> UnBanChatParticipantsAsync(Guid conversationId, List<Guid> userIds)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Participants>> UnBannedParticipantsAsync(Guid conversationId, List<Guid> userIds)
        {
            throw new NotImplementedException();
        }
    }
}
