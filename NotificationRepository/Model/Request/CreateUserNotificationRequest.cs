using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationRepository.Model.Request
{
    public class CreateUserNotificationRequest
    {
        public Guid ConversationId { get; set; }        // Id cuộc trò chuyện
        public Guid receiverId { get; set; }       // Người nhận notification    
        public string Type { get; set; } = string.Empty; // Loại notification (Message, Invite, System,...)
        public string? DataJson { get; set; }
    }
}
