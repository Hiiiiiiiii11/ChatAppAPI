using ChatRepository.Data;
using ChatRepository.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRepository.Repositories
{
    public class ParticipantRepository : IParticipantRepository
    {
        private readonly ChatDbContext _context;
        public ParticipantRepository(ChatDbContext context)
        {
            _context = context;
        }

        public async Task AddParticipantAsync(Participants participant)
        {
            await _context.Participants.AddAsync(participant);
        }

        public Task<Participants?> GetParticipantAsync(Guid conversationId, Guid userId)
        {
            return _context.Participants
                .FirstOrDefaultAsync(p => p.ConversationId == conversationId && p.UserId == userId);
        }

        public async Task<IEnumerable<Participants>> GetParticipantsByConversationIdAsync(Guid conversationId)
        {
            return await _context.Participants
                .Where(p => p.ConversationId == conversationId)
                .ToListAsync();
        }

        public Task RemoveParticipantAsync(Guid conversationId, Guid userId)
        {
            var participant = _context.Participants
                .FirstOrDefaultAsync(p => p.ConversationId == conversationId && p.UserId == userId);
            if (participant != null)
            {
                _context.Participants.Remove(participant.Result);
                return _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("Participant not found.");
            }
        }
        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public Task UpdateParticipantAsync(Participants participant)
        {
            _context.Participants.Update(participant);
            return _context.SaveChangesAsync();
        }
    }
}
