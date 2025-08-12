using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConversationService.Model.Request
{
    public class SendMessageRequest
    {
        public Guid ConversationId { get; set; }
        public Guid SenderId { get; set; }
        public string Content { get; set; }
    }
}
