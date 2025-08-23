using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatService.Model.Request
{
    public class ConversationCreateRequest
    {
        public string? Name { get; set; }
        public bool IsGroup { get; set; }
        public bool IsPrivateGroup { get; set; }
        public bool IsPrivate { get; set; }
    }
}
