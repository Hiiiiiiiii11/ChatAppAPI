﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserRepository.Models;

namespace UserRepository.Repositories
{
    public interface IPasswordResetTokenRepository
    {
        Task AddAsync(PasswordResetToken token);
        Task<PasswordResetToken?> GetValidTokenAsync(Guid userId, string token);
        Task<PasswordResetToken?> GetUnusedValidTokenAsync(Guid userId, string token);
        Task UpdateAsync(PasswordResetToken token);
        Task SaveChangesAsync();
    }

}
