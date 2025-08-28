using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Share.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid? Id
        {
            get
            {
                var userId = _httpContextAccessor.HttpContext?.User.FindFirst("id");
                return userId != null ? Guid.Parse(userId.Value) : null;
            }
        }

        public string? DisplayName => _httpContextAccessor.HttpContext?.User?.FindFirst("displayName")?.Value;

        public string? Email => _httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value;

        public string? Avatar => _httpContextAccessor.HttpContext?.User?.FindFirst("avatarUrl")?.Value;

        public string Role => _httpContextAccessor.HttpContext?.User?.FindFirst("role")?.Value ?? "User";
    }
}
