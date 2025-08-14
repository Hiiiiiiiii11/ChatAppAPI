using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Model.Request
{
    public class ConfirmResetPasswordRequest
    {
        public string Email { get; set; }
        public string Otp { get; set; }
        public string NewPassword { get; set; }
    }
}
