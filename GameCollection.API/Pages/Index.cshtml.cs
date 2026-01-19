// Pages/Index.cshtml.cs
using Microsoft.AspNetCore.Mvc.RazorPages;
using GameCollection.Application.Services;
using GameCollection.Application.DTOs;

namespace GameCollection.API.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IGameService _gameService;

        public List<GameDto> RecentGames { get; set; } = new();
        public List<GameDto> TopRatedGames { get; set; } = new();

        public IndexModel(IGameService gameService)
        {
            _gameService = gameService;
        }

        public async Task OnGetAsync()
        {
            var allGames = await _gameService.GetAllGamesAsync();

            // Recent games (last 6 months)
            RecentGames = allGames
                .Where(g => g.ReleaseYear >= DateTime.Now.Year - 1)
                .Take(6)
                .ToList();

            // Top rated games
            TopRatedGames = allGames
                .Where(g => g.MetacriticScore.HasValue)
                .OrderByDescending(g => g.MetacriticScore)
                .Take(6)
                .ToList();
        }
    }
}