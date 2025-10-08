using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserRepository.Data;
using UserRepository.Models;
using UserService.Repositories;


namespace UserRepository.Repositories
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly UserDbContext _context;

        public AuthenticationRepository(UserDbContext context)
        {
            _context = context;
        }
        public Task<User?> GetUserByEmailAsync(string email)
        {
            return _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
