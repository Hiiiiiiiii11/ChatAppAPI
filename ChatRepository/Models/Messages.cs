using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChatRepository.Models
{
    public class Messages
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ConversationId { get; set; }
        public Guid SenderId { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public bool IsEdited { get; set; } = false;
        public bool IsDeleted { get; set; } = false; // chuyển trạng thái xóa ko thể xem đc
        [JsonIgnore]
        public virtual Conversations Conversation { get; set; }
        [JsonIgnore]
        public virtual ICollection<MessageDeletion> MessageDeletions { get; set; } = new List<MessageDeletion>();
    }
}
