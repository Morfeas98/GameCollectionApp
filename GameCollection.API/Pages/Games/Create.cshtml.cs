using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GameCollection.API.ViewModels;
using GameCollection.Application.Services;
using GameCollection.Domain.Entities;
using GameCollection.Domain.Common;
using GameCollection.Application.DTOs;

namespace GameCollection.API.Pages.Games
{
    public class CreateModel : PageModel
    {
        private readonly IGameService _gameService;
        private readonly ICurrentUserService _currentUser;
        private readonly IRepository<Platform> _platformRepository;
        private readonly IRepository<Genre> _genreRepository;
        private readonly IRepository<Franchise> _franchiseRepository;

        [BindProperty]
        public GameFormViewModel GameForm { get; set; } = new();

        public CreateModel(
            IGameService gameService,
            ICurrentUserService currentUser,
            IRepository<Platform> platformRepository,
            IRepository<Genre> genreRepository,
            IRepository<Franchise> franchiseRepository)
        {
            _gameService = gameService;
            _currentUser = currentUser;
            _platformRepository = platformRepository;
            _genreRepository = genreRepository;
            _franchiseRepository = franchiseRepository;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!_currentUser.IsAuthenticated)
            {
                TempData["ErrorMessage"] = "Please login to add games.";
                return RedirectToPage("/Account/Login", new { returnUrl = "/Games/Create" });
            }

            await LoadDropdownsAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!_currentUser.IsAuthenticated)
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                await LoadDropdownsAsync();
                return Page();
            }

            try
            {
                // Convert ViewModel to DTO
                var createGameDto = new CreateGameDto
                {
                    Title = GameForm.Title,
                    Description = GameForm.Description,
                    ReleaseYear = GameForm.ReleaseYear,
                    Developer = GameForm.Developer,
                    Publisher = GameForm.Publisher,
                    PlatformIds = GameForm.SelectedPlatformIds,
                    GenreIds = GameForm.SelectedGenreIds,
                    FranchiseId = GameForm.FranchiseId,
                    MetacriticScore = GameForm.MetacriticScore,
                    ImageUrl = GameForm.ImageUrl,
                    MetacriticUrl = GameForm.MetacriticUrl
                };

                // Create Game
                var createdGame = await _gameService.CreateGameAsync(createGameDto);

                return RedirectToPage("Details", new { id = createdGame.Id });
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await LoadDropdownsAsync();
                return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await LoadDropdownsAsync();
                return Page();
            }
        }

        private async Task LoadDropdownsAsync()
        {
            var franchises = await _franchiseRepository.GetAllAsync();
            GameForm.Franchises = franchises
                .Where(f => !f.IsDeleted)
                .Select(f => new SelectListItem
                {
                    Value = f.Id.ToString(),
                    Text = f.Name
                })
                .OrderBy(f => f.Text)
                .ToList();

            GameForm.Franchises.Insert(0, new SelectListItem()
            {
                Value = "",
                Text = "-- Select Franchise (Optional) --"
            });

            var platforms = await _platformRepository.GetAllAsync();
            GameForm.Platforms = platforms
                .Where(p => !p.IsDeleted)
                .Select(p => new SelectListItem()
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                })
                .OrderBy(p => p.Text)
                .ToList();

            var genres = await _genreRepository.GetAllAsync();
            GameForm.Genres = genres
                .Where(g => !g.IsDeleted)
                .Select(g => new SelectListItem()
                {
                    Value = g.Id.ToString(),
                    Text = g.Name
                })
                .OrderBy(g => g.Text)
                .ToList();
        }
    }
}
