using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Data;
using UserService.Models;

namespace UserService.Repositories
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly UserDbContext _context;

        public AuthenticationRepository(UserDbContext context)
        {
            _context = context;
        }
        public Task<User> GetUserByEmailAsync(string email)
        {
            return _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
