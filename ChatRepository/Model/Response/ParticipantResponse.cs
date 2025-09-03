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
        public string? DisplayName { get; set; }
        public string? AvatarUrl { get; set; }
        public DateTime JoinAt { get; set; }
    }
}
