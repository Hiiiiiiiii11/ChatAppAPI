using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRepository.Model.Response
{
    public class ParticipantResponse
    {
        public Guid UserId { get; set; }
        public Guid ConversationId { get; set; }
        public string? Email { get; set; }
        public string? DisplayName { get; set; }
        public string? AvatarUrl { get; set; }
        public bool IsActive { get; set; }

        public bool IsBanned { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsBanChat { get; set; }
        public DateTime JoinAt { get; set; }
    }
}
