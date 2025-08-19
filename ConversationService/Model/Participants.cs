using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatService.Models
{
    public class Participants
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public Guid ConversationId { get; set; }

        public bool IsBanned { get; set; } = false;
        public bool IsDeleted { get; set; } = false; // soft delete (xóa chỉ mình tôi)

        public Conversations Conversation { get; set; }
    }
}
