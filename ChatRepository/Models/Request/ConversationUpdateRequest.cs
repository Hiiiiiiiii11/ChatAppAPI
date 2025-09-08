using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRepository.Model.Request
{
    public class ConversationUpdateRequest
    {
        public string? Name { get; set; }
        public bool IsPrivateGroup { get; set; }//danh cho open group co the add paticipants
        public Guid? AdminId { get; set; }
        public IFormFile? AvartarGroup { get; set; }
    }
}
