using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Model.Response
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public DateTime CreatedAt { get; set; }
        public string AvatarUrl { get; set; }
    }
}
