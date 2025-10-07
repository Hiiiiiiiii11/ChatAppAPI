using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserRepository.Models;

namespace UserRepository.Repositories
{
    public interface IEmailVerificationRepository
    {
        Task AddAsync(EmailVerification verification);
        Task<EmailVerification?> GetByEmailAndCodeAsync(string email, string code);
        Task MarkAsVerifiedAsync(EmailVerification verification);
        Task<EmailVerification?> GetByEmailAsync(string email);
    }
}
