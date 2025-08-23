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

        public async Task<Conversations> CreateConversationAsync(Conversations conversation, Guid creatorId, List<Guid> participantIds)
        {
            //neu la chat rieng tu 1-1
            if(!conversation.IsGroup && conversation.IsPrivate)
            {

                
                if (participantIds.Count != 1)
                    throw new InvalidOperationException("Private conversation must be exactly between 2 people.");
                Guid otherUserId = participantIds.First();
                //tìm xem giữa 2 ng có đoạn chat hay ko
                var existing = await _conversationRepository.GetUserConversationsAsync(creatorId);
                var existingPrivate = existing.FirstOrDefault(c =>
                !c.IsGroup &&
                c.IsPrivate &&
                c.Participants.Count == 2 &&
                c.Participants.Any(p => p.UserId == creatorId) &&
                c.Participants.Any(p => p.UserId == otherUserId)

                );
                //nếu có thì sử dụng 
                if(existingPrivate != null)
                {
                    return existingPrivate;
                }
                //nếu chưa có thì tạo mới
                conversation.Participants.Add(new Participants
                {
                    UserId = creatorId,
                    JoinAt = DateTime.UtcNow
                });
                conversation.Participants.Add(new Participants
                {
                    UserId = otherUserId,
                    JoinAt = DateTime.UtcNow
                });
                await _conversationRepository.AddConversationAsync(conversation);
                return conversation;

            }
            if (conversation.IsGroup)
            {
                // thêm người tạo nhóm vào
                conversation.AdminId = creatorId;
                conversation.Participants.Add(new Participants
                {
                    UserId = creatorId,
                    JoinAt = DateTime.UtcNow
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
            throw new InvalidOperationException("Invalid conversation type.");
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
