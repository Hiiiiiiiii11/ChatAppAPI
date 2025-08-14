using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Services
{
    public interface IPasswordResetService
    {
        Task RequestPasswordResetAsync(string email);
        Task ResetPasswordAsync(string email, string otpCode, string newPassword);


    }
}
