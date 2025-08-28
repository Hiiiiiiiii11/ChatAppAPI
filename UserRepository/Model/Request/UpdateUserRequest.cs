using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserRepository.Model.Request
{
    public class UpdateUserRequest
    {
        public string? DisplayName { get; set; }
        public IFormFile? AvatarFile { get; set; }
    }
}
