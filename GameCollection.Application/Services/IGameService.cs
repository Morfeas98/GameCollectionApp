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
    }
}
