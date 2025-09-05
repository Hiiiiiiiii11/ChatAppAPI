


using ChatRepository.Model.Response;
using ChatRepository.Models;
using ChatRepository.Repositories;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace ChatService.Services
{
    public class ParticipantService : IParticipantService
    {
        private readonly IParticipantRepository _participantRepository;
        private readonly IConversationRepository _conversationRepository;

        public ParticipantService(IParticipantRepository participantRepository, IConversationRepository conversationRepository)
        {
            _participantRepository = participantRepository;
            _conversationRepository = conversationRepository;
            
        }

        public async Task<List<Participants>> AddParticipantToConversation(Guid conversationId, List<Guid> userIds)
        {
            var conversation = await _conversationRepository.GetConversationByIdAsync(conversationId);
            if (conversation == null)
            {
                throw new KeyNotFoundException("Conversation not found");
            }

            var addedParticipants = new List<Participants>();

            foreach (var userId in userIds)
            {
                if (userId == Guid.Empty) continue;
                // kiểm tra trùng
                var existing = await _participantRepository.GetParticipantAsync(conversationId, userId);
                if (existing != null) continue;

                var participant = new Participants
                {
                    ConversationId = conversationId,
                    UserId = userId,
                    IsBanned = false,
                    IsBanChat = false,
                    JoinAt = DateTime.UtcNow
                };
                await _participantRepository.AddParticipantAsync(participant);
                addedParticipants.Add(participant);
            }
            return addedParticipants;
        }

        public async Task<IEnumerable<Participants>> BanChatParticipantsAsync(Guid conversationId, List<Guid> userIds)
        {
            var banchatUser = new List<Participants>();
            foreach(var userId in userIds)
            {
                var user = await _participantRepository.GetParticipantAsync(conversationId, userId);
                if (user == null) continue;
                if (user.IsBanChat)
                    throw new Exception("User is banchat already.");
                user.IsBanChat = true;
                await _participantRepository.UpdateParticipantAsync(user);
            }
            return banchatUser;
        }

        public async Task<IEnumerable<Participants>> BannedParticipantsAsync(Guid conversationId, List<Guid> userIds)
        {
            var banUser = new List<Participants>();
            foreach(var userId in userIds)
            {
                var user = await _participantRepository.GetParticipantAsync(conversationId, userId);
                if (user == null) continue;
                if (user.IsBanned)
                    throw new Exception("User is banned already.");
                user.IsBanned = true;
                await _participantRepository.UpdateParticipantAsync(user);

            }
            return banUser;
        }

        public async Task<IEnumerable<Participants>> GetBanChatParticipantsByConversationIdAsync(Guid conversationId)
        {
            return await _participantRepository.GetBanChatParticipantsByConversationIdAsync(conversationId);
        }

        public async Task<IEnumerable<Participants>> GetBannedParticipantsByConversationIdAsync(Guid conversationId)
        {
            return await _participantRepository.GetBannedParticipantsByConversationIdAsync(conversationId);
        }

        public async Task<Participants?> GetParticipantAsync(Guid conversationId, Guid userId)
        {
            return await _participantRepository.GetParticipantAsync(conversationId, userId);
        }

        public async Task<IEnumerable<Participants>> GetParticipantsByConversationIdAsync(Guid conversationId)
        {
            return await _participantRepository.GetParticipantsByConversationIdAsync(conversationId);
        }

        public async Task<IEnumerable<Participants>> RemoveParticipantsAsync(Guid conversationId, List<Guid> userIds)
        {
            var conversation =await _conversationRepository.GetConversationByIdAsync(conversationId);
            if(conversation == null)
            {
                throw new KeyNotFoundException("Conversation not found");
            }

            if (!conversation.IsGroup && conversation.IsPrivate) {
                throw new InvalidOperationException("Cannot remove participant from a private conversation.");
            }

            var removeUser = new List<Participants>();
             foreach(var userId in userIds)
            {
                try
                {
                    await _participantRepository.RemoveParticipantAsync(conversationId, userId);
                    removeUser.Add(new Participants
                    {
                        ConversationId = conversationId,
                        UserId = userId
                    });

                }
                catch (KeyNotFoundException)
                {
                    //bỏ qua user ko tồn tại để tránh lỗi
                    continue;
                }
            }
            return removeUser;
        }


        public async Task<IEnumerable<Participants>> UnBanChatParticipantsAsync(Guid conversationId, List<Guid> userIds)
        {
            var banchatUser = new List<Participants>();
            foreach (var userId in userIds)
            {
                var user = await _participantRepository.GetParticipantAsync(conversationId, userId);
                if (user == null) continue;
                if (!user.IsBanChat)
                    throw new Exception("User is unbanchat already.");

                user.IsBanChat = false;
                await _participantRepository.UpdateParticipantAsync(user);
            }
            return banchatUser;
        }

        public async Task<IEnumerable<Participants>> UnBannedParticipantsAsync(Guid conversationId, List<Guid> userIds)
        {
            var banchatUser = new List<Participants>();
            foreach (var userId in userIds)
            {
                var user = await _participantRepository.GetParticipantAsync(conversationId, userId);
                if (user == null) continue;
                if (!user.IsBanned)
                    throw new Exception("User is unbanned already.");

                user.IsBanned = false;
                await _participantRepository.UpdateParticipantAsync(user);
            }
            return banchatUser;
        }
    }
}
