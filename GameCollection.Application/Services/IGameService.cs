using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameCollection.Application.DTOs;

namespace GameCollection.Application.Services
{
    public interface IGameService
    {
        Task<IEnumerable<GameDto>> GetAllGamesAsync();
        Task<GameDto?> GetGameByIdAsync(int id);
        Task<GameDto> CreateGameAsync(CreateGameDto gameDto);
        Task<GameDto?> UpdateGameAsync(int id, UpdateGameDto gameDto);
        Task<bool> DeleteGameAsync(int id);
        Task<IEnumerable<GameDto>> SearchGamesAsync(string searchTerm);
        Task<IEnumerable<GameDto>> GetRecommendationsAsync(int gameId);
        Task<List<int>> GetGamePlatformIdsAsync(int gameId);
        Task<List<int>> GetGameGenreIdsAsync(int gameId);
        Task<int?> GetGameFranchiseIdAsync(int gameId);

        Task<List<GameDto>> GetRecentTopRatedGamesAsync(int count);
        Task<List<GameDto>> GetTopRatedGamesAsync(int count);

        Task<GamePagedResultDto> GetFilteredGamesAsync(GameQueryParams queryParams);

        //Task<GamePagedResultDto> GetFilteredGamesAsync(
        //    string? searchQuery = null,
        //    int? platformId = null,
        //    int? genreId = null,
        //    int? franchiseId = null,
        //    int? minYear = null,
        //    int? maxYear = null,
        //    string sortBy = "title_asc",
        //    int pageNumber = 1,
        //    int pageSize = 12,
        //    bool includeDeleted = false);
    }
}
