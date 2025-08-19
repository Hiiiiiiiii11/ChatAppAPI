using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatService.Models
{
    public class Conversations
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public bool IsGroup { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsPrivate { get; set; }
        public Guid AdminId { get; set; }

        public virtual ICollection<Participants> Participants { get; set; } = new List<Participants>();
        public virtual ICollection<Messages> Messages { get; set; } = new List<Messages>();

    }
}
