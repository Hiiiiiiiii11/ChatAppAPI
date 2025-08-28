using ChatRepository.Models;
using ChatRepository.Repositories;
using ChatService.Repositories;
using Microsoft.AspNetCore.Components.Forms;
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
        public MessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }
        public async Task<Messages> CreateMessageAsync(Messages message)
        {
            await _messageRepository.AddMessageAsync(message);
            await _messageRepository.SaveChangesAsync();
            return message;
        }

        public async Task DeleteMessageAsync(Guid id)
        {
             await _messageRepository.DeleteMessageAsync(id);
        }

        public async Task<Messages?> GetMessageByIdAsync(Guid id)
        {
           return  await _messageRepository.GetMessageByIdAsync(id);
        }

        public async Task<IEnumerable<Messages>> GetMessageByRoomIdAsync(Guid conversationId, int? take = null, DateTime? before = null)
        {
            return await _messageRepository.GetMessagesByRoomIdAsync(conversationId, take, before);
        }

        public async Task UpdateMessageAsync(Messages message)
        {
            await _messageRepository.UpdateMessageAsync(message);
        }
    }
}
