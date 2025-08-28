using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRepository.Model.Request
{
    public class ConversationGroupUpdateRequest
    {
        public string Name { get; set; }
        public bool IsPrivateGroup { get; set; }//danh cho open group co the add paticipants
        public Guid? AdminId { get; set; }
    }
}
