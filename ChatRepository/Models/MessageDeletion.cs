using ChatRepository.Models;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChatRepository.Models
{
    public class MessageDeletion
    {
        //xóa tin nhắn chỉ mình tôi
        public Guid Id { get; set; }
        public Guid MessageId { get; set; }
        public Guid UserId { get; set; }
        public DateTime DeletedAt { get; set; }

        // Navigation properties
        [JsonIgnore]
        public Messages Message { get; set; } = null!;
    }
}
