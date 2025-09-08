using ChatRepository.Model.Request;
using ChatRepository.Model.Response;
using ChatRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatService.Services
{
    public interface IConversationService
    {
        Task<ConversationResponse> GetConversationByIdAsync(Guid id);
        Task<IEnumerable<ConversationResponse>> GetUserConversationsAsync(Guid userId);
        Task<ConversationResponse> CreateConversationAsync(ConversationCreateRequest request, Guid creatorId);
        Task UpdateConversationAsync(Guid id,ConversationUpdateRequest request, Guid adminGroupId);
        Task DeleteConversationAsync(Guid id);
        Task DissolveConversationAsync(Guid id);
        Task<IEnumerable<ConversationResponse>> SearchConversationsAsync(Guid userId, string conversationName);

    }
}
