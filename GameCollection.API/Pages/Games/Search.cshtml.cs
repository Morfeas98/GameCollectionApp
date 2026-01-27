using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GameCollection.Application.Services;
using GameCollection.Application.DTOs;
using GameCollection.Domain.Repositories;
using GameCollection.Domain.Common;
using GameCollection.Domain.Entities;

namespace GameCollection.API.Pages.Games
{
    public class SearchModel : PageModel
    {
        private readonly IGameService _gameService;
        private readonly IRepository<Platform> _platformRepository;
        private readonly IRepository<Genre> _genreRepository;
        private readonly IRepository<Franchise> _franchiseRepository;

        public List<GameDto> SearchResults { get; set; } = new();
        public string SearchQuery { get; set; }
        public List<SelectListItem> Platforms { get; set; } = new();
        public List<SelectListItem> Genres { get; set; } = new();
        public List<SelectListItem> Franchises { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public SearchFilters Filters { get; set; } = new();

        public SearchModel(
            IGameService gameService,
            IRepository<Platform> platformRepository,
            IRepository<Genre> genreRepository,
            IRepository<Franchise> franchiseRepository)
        {
            _gameService = gameService;
            _platformRepository = platformRepository;
            _genreRepository = genreRepository;
            _franchiseRepository = franchiseRepository;
        }

        public async Task OnGetAsync()
        {
            await LoadDropdownsAsync();

            if (!string.IsNullOrEmpty(Filters.Query) ||
                Filters.PlatformId.HasValue ||
                Filters.GenreId.HasValue ||
                Filters.MinYear.HasValue ||
                Filters.MaxYear.HasValue)
            {
                await PerformSearchAsync();
            }
        }

        private async Task LoadDropdownsAsync()
        {
            var platforms = await _platformRepository.GetAllAsync();
            Platforms = platforms
                .Where(p => !p.IsDeleted)
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                })
                .OrderBy(p => p.Text)
                .ToList();

            Platforms.Insert(0, new SelectListItem { Value = "", Text = "All Platforms" });

            var genres = await _genreRepository.GetAllAsync();
            Genres = genres
                .Where(g => !g.IsDeleted)
                .Select(g => new SelectListItem
                {
                    Value = g.Id.ToString(),
                    Text = g.Name
                })
                .OrderBy(g => g.Text)
                .ToList();

            Genres.Insert(0, new SelectListItem { Value = "", Text = "All Genres" });

            var franchises = await _franchiseRepository.GetAllAsync();
            Franchises = franchises
                .Where(f => !f.IsDeleted)
                .Select(f => new SelectListItem
                {
                    Value = f.Id.ToString(),
                    Text = f.Name
                })
                .OrderBy(f => f.Text)
                .ToList();

            Franchises.Insert(0, new SelectListItem { Value = "", Text = "All Franchises" });
        }

        private async Task PerformSearchAsync()
        {
            var allGames = await _gameService.GetAllGamesAsync();
            var filteredGames = allGames.AsEnumerable();

            if (!string.IsNullOrEmpty(Filters.Query))
            {
                filteredGames = filteredGames
                    .Where(g => g.Title.Contains(Filters.Query, StringComparison.OrdinalIgnoreCase) ||
                        (g.Description != null && g.Description.Contains(Filters.Query, StringComparison.OrdinalIgnoreCase)) ||
                        (g.Developer != null && g.Developer.Contains(Filters.Query, StringComparison.OrdinalIgnoreCase)));
            }

            if (Filters.PlatformId.HasValue)
            {
                // TEMP - NEED SERVICE METHOD
            }

            if (Filters.GenreId.HasValue)
            {
                // TEMP - NEED SERVICE METHOD
            }

            if (Filters.MinYear.HasValue)
            {
                filteredGames = filteredGames.Where(g => g.ReleaseYear >= Filters.MinYear.Value);
            }

            if (Filters.MaxYear.HasValue)
            {
                filteredGames = filteredGames.Where(g => g.ReleaseYear <= Filters.MaxYear.Value);
            }

            SearchResults = filteredGames.ToList();
        }

        public class SearchFilters
        {
            public string Query { get; set; }
            public int? PlatformId { get; set; }
            public int? GenreId { get; set; }
            public int? FranchiseId { get; set; }
            public int? MinYear { get; set; }
            public int? MaxYear { get; set; }
            public int? MinRating { get; set; }
            public int? MaxRating { get; set; }
            public string SortBy { get; set; } = "title";
        }
    }
}
