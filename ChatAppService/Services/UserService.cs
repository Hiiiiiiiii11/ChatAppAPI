using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Models;
using UserService.Repositories;

namespace UserService.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }



        public async Task<User> AddUserAsync(User user)
        {
            var existUser = await _userRepository.GetUserByEmailAsync(user.Email);

            if(existUser != null)
            {
                throw new InvalidOperationException($"User with email {user.Email} already exists.");
            }
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
            var newUser = new User
            {
                Email = user.Email,
                PasswordHash = hashedPassword,
                DisplayName = user.DisplayName,
                CreatedAt = DateTime.UtcNow,
                AvatarUrl = !string.IsNullOrEmpty(user.AvatarUrl) ? user.AvatarUrl : null,
                IsActive = true
            };

            await _userRepository.AddAsync(newUser);

            return newUser; 
        }

        public async Task DeleteUserAsync(Guid id)
        {
            await _userRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<User>> SearchUsersAsync(string searchTerm)
        {
            return await _userRepository.GetAllAsync()
                .ContinueWith(task => task.Result
                    .Where(u => u.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)));
        }

        public async Task UnActiveUser(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with id {id} not found.");
            }
            if(user.IsActive == false)
            {
                throw new Exception("User status is unactive already.");
            }    

            user.IsActive = false;
            await _userRepository.UpdateAsync(user);
        }
        public async Task ActiveUser(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with id {id} not found.");
            }
            if (user.IsActive == true)
            {
                throw new Exception("User status is active already.");
            }

            user.IsActive = true;
            await _userRepository.UpdateAsync(user);
        }

        public async Task UpdateUserAsync(User user)
        {
            await _userRepository.UpdateAsync(user);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var existUser = await _userRepository.GetUserByEmailAsync(email);
            return existUser ?? throw new KeyNotFoundException($"User with email {email} not found.");
        }
    }
}
