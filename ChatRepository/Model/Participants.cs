using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChatRepository.Models
{
    public class Participants
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public Guid ConversationId { get; set; }

        public bool IsBanned { get; set; } = false; //chặn thành viên
        public bool IsDeleted { get; set; } = false; // soft delete (xóa chỉ mình tôi)
        public bool IsBanChat { get; set; } = false; //chặn chat
        public DateTime JoinAt { get; set; } = DateTime.UtcNow;
        [JsonIgnore]
        public Conversations Conversation { get; set; }
    }
}
