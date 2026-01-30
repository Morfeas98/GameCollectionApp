using GameCollection.Application.DTOs;
using GameCollection.Application.Services;
using GameCollection.Domain.Common;
using GameCollection.Domain.Entities;
using GameCollection.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GameCollection.API.Pages.Games
{
    public class IndexModel : PageModel
    {
        private readonly IGameService _gameService;
        private readonly IRepository<Platform> _platformRepository;
        private readonly IRepository<Genre> _genreRepository;
        private readonly IRepository<Franchise> _franchiseRepository;


        public GamePagedResultDto? GameResult { get; set; }
        public List<SelectListItem> PlatformOptions { get; set; } = new();
        public List<SelectListItem> GenreOptions { get; set; } = new();
        public List<SelectListItem> FranchiseOptions { get; set; } = new();

        
        [BindProperty(SupportsGet = true)]
        public GameQueryParams Query { get; set; } = new();

        public IndexModel(
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

            GameResult = await _gameService.GetFilteredGamesAsync(Query);
        }

        private async Task LoadDropdownsAsync()
        {
            // Load Platforms
            var platforms = await _platformRepository.GetAllAsync();
            PlatformOptions = platforms
                .Where(p => !p.IsDeleted)
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                })
                .OrderBy(p => p.Text)
                .ToList();
            PlatformOptions.Insert(0, new SelectListItem
            {
                Value = "",
                Text = "All Platforms",
                Selected = !Query.PlatformId.HasValue
            });

            // Load Genres
            var genres = await _genreRepository.GetAllAsync();
            GenreOptions = genres
                .Where(g => !g.IsDeleted)
                .Select(g => new SelectListItem
                {
                    Value = g.Id.ToString(),
                    Text = g.Name
                })
                .OrderBy(g => g.Text)
                .ToList();
            GenreOptions.Insert(0, new SelectListItem
            {
                Value = "",
                Text = "All Genres",
                Selected = !Query.GenreId.HasValue
            });

            // Load Franchises
            var franchises = await _franchiseRepository.GetAllAsync();
            FranchiseOptions = franchises
                .Where(f => !f.IsDeleted)
                .Select(f => new SelectListItem
                {
                    Value = f.Id.ToString(),
                    Text = f.Name
                })
                .OrderBy(f => f.Text)
                .ToList();
            FranchiseOptions.Insert(0, new SelectListItem
            {
                Value = "",
                Text = "All Franchises",
                Selected = !Query.FranchiseId.HasValue  
            });
        }
        public bool HasActiveFilters()
        {
            return Query?.HasFilters() ?? false;
        }
    }
}
