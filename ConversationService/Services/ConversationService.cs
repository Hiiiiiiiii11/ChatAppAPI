using ChatService.Models;
using ChatService.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatService.Services
{
    public class ConversationService : IConversationService
    {
        private readonly IConversationRepository _conversationRepository;
        public ConversationService(IConversationRepository conversationRepository)
        {
            _conversationRepository = conversationRepository;
        }

        public async Task<Conversations> CreateConversationAsync(Conversations conversation, Guid adminId, List<Guid> participantIds)
        {
            //neu la chat rieng tu 1-1
            if(!conversation.IsGroup && conversation.IsPrivate && conversation.Participants.Count == 2)
            {
                var existing = await _conversationRepository.GetUserConversationsAsync(participantIds[0]);
                var existingPrivate = existing.FirstOrDefault(c => !c.IsGroup && c.IsPrivate && c.Participants.Count == 2 && c.Participants.All(p => participantIds.Contains(p.UserId)));
                if (existingPrivate != null)
                {
                    return existingPrivate;                
                }
                
            }
            // thêm người tạo nhóm vào
            conversation.Participants.Add(new Participants
            {
                UserId = adminId,
                ConversationId = conversation.Id,
                JoinAt =DateTime.UtcNow
            });

            
            // Nếu chưa có thì tạo mới foreach (var userId in participantIds)
            foreach (var userId in participantIds)
            {
                conversation.Participants.Add(new Participants
                {
                    UserId = userId,
                    ConversationId = conversation.Id,
                    JoinAt = DateTime.UtcNow
                });
            }
            await _conversationRepository.AddConversationAsync(conversation);
            return conversation;
        }

        public async Task DeleteConversationAsync(Guid id)
        {
            await _conversationRepository.DeleteConversationAsync(id);
        }

        public async Task<Conversations?> GetConversationByIdAsync(Guid id)
        {
           return await _conversationRepository.GetConversationByIdAsync(id);
        }

        public async Task<IEnumerable<Conversations>> GetUserConversationsAsync(Guid userId)
        {
           return await _conversationRepository.GetUserConversationsAsync(userId);
        }

        public async Task UpdateConversationAsync(Conversations conversation)
        {
            await _conversationRepository.UpdateConversationAsync(conversation);
        }
    }
}
