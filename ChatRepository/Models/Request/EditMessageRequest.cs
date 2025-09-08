using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRepository.Models.Request
{
    public class EditMessageRequest
    {
       public string? NewContent { get; set; } = string.Empty;
    }
}
