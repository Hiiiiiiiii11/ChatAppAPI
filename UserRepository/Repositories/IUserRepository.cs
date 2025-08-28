﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserRepository.Models;



namespace UserRepository.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(Guid id);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(Guid id);
        Task<User?> GetUserByEmailAsync(string email);
        Task<List<User>> SearchAsync(string searchTerm);
        Task UnActiveUser(Guid id);
        Task ActiveUser(Guid id);
    }
}
