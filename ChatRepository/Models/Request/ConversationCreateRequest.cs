using ChatRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRepository.Model.Request
{
    public class ConversationCreateRequest
    {
        public string? Name { get; set; }
        public bool IsGroup { get; set; }
        public bool IsPrivateGroup { get; set; }
        public bool IsPrivate { get; set; }
        public List<Guid> ParticipantIds { get; set; } = new();
    }
}
