using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Services
{
    public interface ICurrentUserService
    {
         Guid? Id { get;  }
         string? DisplayName { get;  }
         string? Email { get;  }
         string? Avatar { get;  }
         string Role { get;  }
    }
}
