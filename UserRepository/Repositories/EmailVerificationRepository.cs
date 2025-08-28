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
    public class EmailVerificationRepository : IEmailVerificationRepository
    {
        private readonly UserDbContext _context;

        public EmailVerificationRepository(UserDbContext context)
        {
            _context = context;
        }

        public  async Task AddAsync(EmailVerification verification)
        {
            _context.EmailVerifications.Add(verification);
            await _context.SaveChangesAsync();
        }

        public async Task<EmailVerification?> GetByEmailAndCodeAsync(string email, string code)
        {
            return await _context.EmailVerifications
                .FirstOrDefaultAsync(e =>
                    e.Email == email &&
                    e.Code == code &&
                    e.IsVerified == false);
        }

        public async Task MarkAsVerifiedAsync(EmailVerification verification)
        {
            verification.IsVerified = true;
            _context.EmailVerifications.Update(verification);
            await _context.SaveChangesAsync();
        }
        public async Task<EmailVerification?> GetByEmailAsync(string email)
        {
            return await _context.EmailVerifications
                .OrderByDescending(e => e.ExpiredAt)
                .FirstOrDefaultAsync(e => e.Email == email);
        }
    }
}
