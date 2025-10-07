using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserRepository.Model.Request;

namespace UserService.Services
{
    public interface IPasswordResetService
    {
        Task RequestPasswordResetAsync(PasswordResetRequest request);
        Task ResetPasswordAsync(ConfirmResetPasswordRequest request);


    }
}
