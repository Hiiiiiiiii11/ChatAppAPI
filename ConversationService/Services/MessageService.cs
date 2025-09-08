using ChatRepository.Models;
using ChatRepository.Models.Request;
using ChatRepository.Models.Response;
using ChatRepository.Repositories;
using ChatService.Mapping;
using ChatService.Repositories;
using Microsoft.AspNetCore.Components.Forms;
using Share.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatService.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IConversationRepository _conversationRepository;
        public MessageService(IMessageRepository messageRepository, IConversationRepository conversationRepository)
        {
            _messageRepository = messageRepository;
            _conversationRepository = conversationRepository;   
        }
        public async Task<MessageResponse> SendMessageAsync(SendMessageRequest request, Guid senderId)
        {
            var conversation = await _conversationRepository.GetConversationByIdAsync(request.ConversationId);

            if (conversation == null)
            {
                throw new Exception("Conversation not found.");
            }

            // 2. Kiểm tra trạng thái
            if (conversation.IsDissolve) // hoặc IsActive == false
            {
                throw new Exception("Conversation has been dissolved. Cannot send messages.");
            }
            var message = new Messages
            {
                ConversationId = request.ConversationId,
                SenderId = senderId,
                Content = request.Content,
                SentAt = DateTime.UtcNow,
                IsEdited = false,
                IsDeleted = false
            };
           await _messageRepository.SendMessageAsync(message);
            await _messageRepository.SaveChangesAsync();
            return message.MessageToResponse();
        }

        public async Task DeleteMessageAsync(Guid id)
        {
             await _messageRepository.DeleteMessageAsync(id);
        }

        public async Task DeleteMessageOnlyUserAsync(Guid id, Guid userId)
        {
            var message = await _messageRepository.GetMessageByIdAsync(id);
            if (message == null) throw new KeyNotFoundException("Message not found");

            var deletion = new MessageDeletion
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                MessageId = message.Id,
                DeletedAt = DateTime.UtcNow
            };

            await _messageRepository.DeleteMessageOnlyUserAsync(deletion);
            await _messageRepository.SaveChangesAsync();
        }
           

        public async Task DeleteMessageWithAllAsync(Guid id)
        {
            var message = await _messageRepository.GetMessageByIdAsync(id);
            if (message == null) throw new KeyNotFoundException("Message not found");

            message.IsDeleted = true;
            await _messageRepository.DeleteMessageWithAllAsync(message);
            await _messageRepository.SaveChangesAsync();
        }

        public async Task<MessageResponse?> GetMessageByIdAsync(Guid id)
        {
            var message = await _messageRepository.GetMessageByIdAsync(id);
            return message?.MessageToResponse();
        }

        public async Task<IEnumerable<MessageResponse>> GetMessageByRoomIdAsync(Guid conversationId, Guid currentUserId, int? take = null, DateTime? before = null)
        {
            var messages = await _messageRepository.GetMessagesByRoomIdAsync(conversationId, take, before);
            return messages.Select(m =>
            {
                string content;
                if ((m.IsDeleted))
                {
                    content = "Message has been removed.";
                }else if(m.MessageDeletions != null && m.MessageDeletions.Any(md => md.UserId == currentUserId))
                {
                    content = "You has been removed this message.";
                }
                else
                {
                    content = m.IsEdited ? $"{m.Content}(edited)": m.Content;
                }
                return new MessageResponse
                {
                    Id = m.Id,
                    ConversationId = m.ConversationId,
                    SenderId = m.SenderId,
                    SentAt = m.SentAt,
                    Content = content,
                    IsEdited = m.IsEdited,
                    IsDeleted = m.IsDeleted
                };
            });
        }

        public async Task<IEnumerable<MessageResponse>> SearchMessagesAsync(Guid conversationId, string keyword, int? take = null, DateTime? before = null)
        {
            var searchMessage = await _messageRepository.SearchMessageAsync(conversationId, keyword, take, before);
            return searchMessage.Select(m => m.MessageToResponse());
        }

        public async Task EditMessageAsync(Guid id, EditMessageRequest request)
        {
            var message = await _messageRepository.GetMessageByIdAsync(id);
            if (message == null)
                throw new KeyNotFoundException("Message not found.");
            message.Content = request.NewContent;
            message.IsEdited = true;
            await _messageRepository.EditMessageAsync(message);
            await _messageRepository.SaveChangesAsync();
        }
    }
}
