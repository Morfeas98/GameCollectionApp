using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GameCollection.Application.Services;
using GameCollection.Application.DTOs;

namespace GameCollection.API.Pages.Games
{
    public class IndexModel : PageModel
    {
        private readonly IGameService _gameService;

        public List<GameDto> Games { get; set; }
        public int TotalCount { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 12; 
        public string SearchQuery { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? PlatformId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? GenreId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? MinYear { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? MaxYear { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SortBy { get; set; } = "title";

        public List<PlatformDto> Platforms { get; set; } = new();
        public List<GenreDto> Genres { get; set; } = new();

        public IndexModel(IGameService gameService)
        {
            _gameService = gameService;
        }

        public async Task OnGetAsync(
            int page = 1,
            string search = null,
            string sort = "title")
        {
            PageNumber = page;
            SearchQuery = search;
            SortBy = sort;

            var allGames = await _gameService.GetAllGamesAsync();

            // Apply Search Filter
            if (!string.IsNullOrEmpty(SearchQuery))
            {
                allGames = (await _gameService.SearchGamesAsync(SearchQuery)).ToList();
            }

            // TODO: additional filters
            // TODO: sorting

            // Apply Pagination
            Games = allGames
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            TotalCount = allGames.Count();
        }
    }

    public class PlatformDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class GenreDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
