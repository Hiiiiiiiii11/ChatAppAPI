using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationRepository.Model.Request
{
    public class CreateNotidicationRequest
    {
        public Guid UserId { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public object? Data { get; set; }
    }
}
