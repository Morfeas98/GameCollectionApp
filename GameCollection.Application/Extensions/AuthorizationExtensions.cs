using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using GameCollection.Domain.Entities;

namespace GameCollection.Application.Extensions
{
    public static class AuthorizationExtensions
    {
        public static bool IsAdmin(this ClaimsPrincipal user)
        {
            return user.IsInRole("Admim");
        }

        public static bool IsModerator(this ClaimsPrincipal user)
        {
            return user.IsInRole("Moderator") || user.IsInRole("Admin");
        }

        public static bool IsOwner(this ClaimsPrincipal user, int ownerId)
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return false;

            return userId == ownerId;
        }
    }
}
