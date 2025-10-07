using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRepository.Models.Request
{
    public class SendGroupMessageRequest
    {
        public Guid ConversationId { get; set; }
        public string Content { get; set; } = null!;
    }
}
