using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Models;

namespace UserService.Services
{
    public class Authentication : IAuthentication
    {
        public async Task<User?> AuthenticateUserAsync(string username, string password)
        {
           
        }
    }
}
