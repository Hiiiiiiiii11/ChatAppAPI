using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppBussinessObject.ResponseModel
{
    public class MessageResponse
    {
        public Guid Id { get; set; }
        public Guid SenderId { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; }
    }
}
