using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppBussinessObject.Models
{
    public class Conversations
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<Participants> Participants { get; set; } = new List<Participants>();
        public ICollection<Messages> Messages { get; set; } = new List<Messages>();

    }
}
