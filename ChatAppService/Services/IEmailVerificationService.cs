using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserRepository.Model.Request;

namespace UserService.Services
{
    public interface IEmailVerificationService
    {
        Task SendVerificationCodeAsync(string email);
        Task<bool> VerifyCodeAsync(VerifyOTPRequest request);
        Task SendPasswordResetCodeAsync(string email, string otp);
        Task<bool> IsEmailVerifiedAsync(string email);
        Task SendPasswordChangedNotificationAsync(string email);

    }
}
