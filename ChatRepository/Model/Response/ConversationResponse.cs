using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRepository.Model.Response
{
    public class ConversationResponse
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public bool IsGroup { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsPrivateGroup { get; set; }//danh cho open group co the add paticipants
        public bool IsPrivate { get; set; }//chi danh cho chat 1-1
        public Guid? AdminId { get; set; }
        public bool IsDissolve { get; set; }
        public List<ParticipantResponse> Participants { get; set; } = new();
    }
}
