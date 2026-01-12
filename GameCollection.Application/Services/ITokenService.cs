using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameCollection.Domain.Entities;

namespace GameCollection.Application.Services
{
    public interface ITokenService
    {
        string CreateToken(User user);
        int? ValidateToken(string token);
    }
}
