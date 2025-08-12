using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppBussinessObject.RequestModel
{
    public class CreateGroupConversationRequest
    {
        public string Name { get; set; }
        public Guid CreatorUserId { get; set; }
    }
}
