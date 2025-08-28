using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserRepository.Models;


namespace UserService.Services
{
    public interface IAuthenticationService
    {
        Task<User> AuthenticateUserAsync(string username, string password);
        string GenerateTokenAsync(User user,string role);
    }
}
