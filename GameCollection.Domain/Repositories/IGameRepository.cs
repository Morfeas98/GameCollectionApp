using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameCollection.Domain.Common;
using GameCollection.Domain.Entities;

namespace GameCollection.Domain.Repositories
{
    public interface IGameRepository : IRepository<Game>
    {
        Task<IEnumerable<Game>> GetGamesWithDetailsAsync();
        Task<Game?> GetGameWithDetailsAsync(int id);
        Task<IEnumerable<Game>> SearchGamesAsync(string searchTerm);
        Task<IEnumerable<Game>> GetGamesByPlatformAsync(int platformId);
        Task<IEnumerable<Game>> GetGamesByGenreAsync(int genreId);
        Task<IEnumerable<Game>> GetGamesByFranchiseAsync(int franchiseId);
    }
}
