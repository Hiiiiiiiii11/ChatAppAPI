using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Services
{
    public interface IEmailVerificationService
    {
        Task SendVerificationCodeAsync(string email);
        Task<bool> VerifyCodeAsync(string email, string code);
        Task SendPasswordResetCodeAsync(string email, string otp);
        Task<bool> IsEmailVerifiedAsync(string email);
        Task SendPasswordChangedNotificationAsync(string email);

    }
}
