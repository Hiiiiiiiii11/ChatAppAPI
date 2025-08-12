using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppBussinessObject.RequestModel
{
    public class SendMessageRequest
    {
        public Guid ConversationId { get; set; }
        public Guid SenderUserId { get; set; }
        public string Content { get; set; } 
    }
}
