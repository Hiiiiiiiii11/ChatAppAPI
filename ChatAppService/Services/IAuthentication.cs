using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Models;

namespace UserService.Services
{
    public interface IAuthentication
    {
        Task<User?> AuthenticateUserAsync(string username, string password);
    }
}
