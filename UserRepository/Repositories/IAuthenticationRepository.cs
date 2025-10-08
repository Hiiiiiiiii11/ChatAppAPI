using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserRepository.Models;


namespace UserService.Repositories
{
    public interface IAuthenticationRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
    }
}
