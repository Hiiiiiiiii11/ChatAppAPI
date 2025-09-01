using Microsoft.AspNetCore.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserRepository.Model.Request;
using UserRepository.Model.Response;
using UserRepository.Models;


namespace UserService.Services
{
    public interface IAuthenticationService
    {
        Task<AuthResponse> LoginAsync(LoginUserRequest request, string adminEmail);
        string GenerateTokenAsync(User user,string role);
    }
}
