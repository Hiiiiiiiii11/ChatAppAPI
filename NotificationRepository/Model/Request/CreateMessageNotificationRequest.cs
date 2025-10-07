using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationRepository.Model.Request
{
    public class CreateMessageNotificationRequest
    {
        public Guid ConversationId { get; set; }        // Id cuộc trò chuyện
        public Guid MessageId { get; set; }             // Id tin nhắn gốc (nếu có)            // Người nhận notification
        public string Type { get; set; } = string.Empty; // Loại notification (Message, Invite, System,...)
        public string? DataJson { get; set; }
    }
}
