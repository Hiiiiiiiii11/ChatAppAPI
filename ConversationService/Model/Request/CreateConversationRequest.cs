using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConversationService.Model.Request
{
    public class CreateConversationRequest
    {
        public string Name { get; set; }
        public Guid CreatorUserId { get; set; }
    }
}
