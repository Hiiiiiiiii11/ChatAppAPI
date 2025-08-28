using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserRepository.Data;
using UserRepository.Models;


namespace UserRepository.Repositories
{
    public class PasswordResetTokenRepository : IPasswordResetTokenRepository
    {
        public readonly UserDbContext _context;
        public PasswordResetTokenRepository(UserDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(PasswordResetToken token)
        {
            await _context.PasswordResetTokens.AddAsync(token);
        }

        public async Task<PasswordResetToken?> GetValidTokenAsync(Guid userId, string token)
        {
            return await _context.PasswordResetTokens
             .FirstOrDefaultAsync(t => t.UserId == userId && t.Token == token && !t.IsUsed);
        }

        public async Task<PasswordResetToken?> GetUnusedValidTokenAsync(Guid userId, string token)
        {
            return await _context.PasswordResetTokens
                .FirstOrDefaultAsync(t => t.UserId == userId
                                           && t.Token == token
                                           && !t.IsUsed
                                           && t.ExpiredAt > DateTime.UtcNow);
        }
        public async Task UpdateAsync(PasswordResetToken token)
        {
            _context.PasswordResetTokens.Update(token);
        }


        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
