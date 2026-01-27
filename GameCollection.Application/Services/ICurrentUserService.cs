using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCollection.Application.Services
{
    public interface ICurrentUserService
    {
        int? UserId { get; }
        string? Username { get; }
        string? Email { get; }
        bool IsAuthenticated { get; }
        int GetRequiredUserId();
        bool IsInRole(string role);
    }
}
