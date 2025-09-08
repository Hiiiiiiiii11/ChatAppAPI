
using ChatRepository.Models;
using ChatRepository.Models.Request;
using ChatRepository.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatService.Services
{
    public interface IMessageService
    {
        Task<MessageResponse?> GetMessageByIdAsync(Guid id);
        Task<IEnumerable<MessageResponse>> GetMessageByRoomIdAsync(Guid conversationId, Guid currentUserId, int? take = null, DateTime? before = null);
        Task<MessageResponse> SendMessageAsync(SendMessageRequest request, Guid senderId);
        Task EditMessageAsync(Guid id,EditMessageRequest request);
        Task DeleteMessageAsync(Guid id);
        Task<IEnumerable<MessageResponse>> SearchMessagesAsync(Guid conversationId, string keyword, int? take = null, DateTime? before = null);
        Task DeleteMessageOnlyUserAsync(Guid id, Guid userId);
        Task DeleteMessageWithAllAsync(Guid id);
    }
}
