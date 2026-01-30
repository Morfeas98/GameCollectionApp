// Pages/Index.cshtml.cs
using Microsoft.AspNetCore.Mvc.RazorPages;
using GameCollection.Application.Services;
using GameCollection.Application.DTOs;

namespace GameCollection.API.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IGameService _gameService;

        public List<GameDto> RecentTopRatedGames { get; set; } = new();
        public List<GameDto> AllTimeTopRatedGames { get; set; } = new();
        public int CurrentYear { get; set; }

        public IndexModel(IGameService gameService)
        {
            _gameService = gameService;
        }

        public async Task OnGetAsync()
        {
            CurrentYear = DateTime.UtcNow.Year;
            RecentTopRatedGames = await _gameService.GetRecentTopRatedGamesAsync(6);
            AllTimeTopRatedGames = await _gameService.GetTopRatedGamesAsync(6);
        }
    }
}