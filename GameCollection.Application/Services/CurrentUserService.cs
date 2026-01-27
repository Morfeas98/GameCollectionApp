using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace GameCollection.Application.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

        public int? UserId
        {
            get
            {
                var userIdClaims = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return int.TryParse(userIdClaims, out int userId) ? userId : (int?)null;
            }
        }

        public string? Username => User?.FindFirst(ClaimTypes.Name)?.Value;
        public string? Email => User?.FindFirst(ClaimTypes.Email)?.Value;

        public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;

        public int GetRequiredUserId()
        {
            var userId = UserId;
            if (userId == null)
                throw new UnauthorizedAccessException("User not authenticated");

            return userId.Value;
        }
        
        public bool IsInRole(string role)
        {
            return User?.IsInRole(role) ?? false;
        }

        public bool IsAdmin => IsInRole("Admin");
        public bool IsModerator => IsInRole("Moderator") || IsInRole("Admin");
    }
}
