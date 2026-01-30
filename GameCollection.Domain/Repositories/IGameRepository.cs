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
        Task<(IEnumerable<Game> Games, int TotalCount)> GetFilteredGamesAsync(
            string? searchTerm = null,
            int? platformId = null,
            int? genreId = null,
            int? franchiseId = null,
            int? minYear = null,
            int? maxYear = null,
            string sortBy = "title_asc",
            int pageNumber = 1,
            int pageSize = 12,
            bool includeDeleted = false);
    }
}
