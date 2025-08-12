using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppBussinessObject.ResponseModel
{
    public class ConversationResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Guid> ParticipantUserIds { get; set; } = new List<Guid>();
        public DateTime CreatedAt { get; set; }
        public Guid CreatedByUserId { get; set; }
        public bool IsGroupConversation { get; set; }
        // Additional properties can be added as needed
    }
}
