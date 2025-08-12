using ChatService.Models;
using ChatService.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatService.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;
        public ChatService(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }
        public async Task AddMessageAsync(Messages message)
        {
            await _chatRepository.AddMessageAsync(message);
        }

        public async Task DeleteMessageAsync(Guid id)
        {
            await _chatRepository.DeleteMessageAsync(id);
        }

        public async Task<Messages?> GetMessageByIdAsync(Guid id)
        {
            return await _chatRepository.GetMessageByIdAsync(id);
        }

        public Task<IEnumerable<Messages>> GetMessagesByRoomIdAsync(Guid conversationId)
        {
            return _chatRepository.GetMessagesByRoomIdAsync(conversationId);
        }

        public async Task<IEnumerable<Messages>> SearchMessagesAsync(Guid conversationId, string searchTerm)
        {
            return await _chatRepository.GetMessagesByRoomIdAsync(conversationId)
                .ContinueWith(task => task.Result
                    .Where(m => m.Content.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)));
        }

        public async Task UpdateMessageAsync(Messages message)
        {
            await _chatRepository.UpdateMessageAsync(message);
        }
    }
}
