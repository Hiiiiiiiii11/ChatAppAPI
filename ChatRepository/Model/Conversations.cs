using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRepository.Models
{
    public class Conversations
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Name { get; set; }
        public bool IsGroup { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsPrivateGroup { get; set; }//danh cho open group co the add paticipants
        public bool IsPrivate { get; set; }//chi danh cho chat 1-1
        public Guid? AdminId { get; set; }
        public bool IsDissolve { get; set; }// hiển thị nhóm đã giải tán
        //public bool IsHidden { get; set; }// xóa mềm đoạn hội thoai
        public virtual ICollection<Participants> Participants { get; set; } = new List<Participants>();
        public virtual ICollection<Messages> Messages { get; set; } = new List<Messages>();

    }
}
