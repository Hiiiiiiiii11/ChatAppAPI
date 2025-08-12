using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Model.Response
{
    public class AuthResponse
    {
        public string Token { get; set; }
        public UserResponse User { get; set; }
    }
}
