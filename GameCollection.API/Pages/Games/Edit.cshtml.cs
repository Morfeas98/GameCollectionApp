using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GameCollection.API.ViewModels;
using GameCollection.Application.Services;
using GameCollection.Domain.Entities;
using GameCollection.Domain.Common;
using GameCollection.Application.DTOs;
using Microsoft.AspNetCore.Components;

namespace GameCollection.API.Pages.Games
{
    public class EditModel : PageModel
    {
        private readonly IGameService _gameService;
        private readonly ICurrentUserService _currentUser;
        private readonly IRepository<Platform> _platformRepository;
        private readonly IRepository<Genre> _genreRepository;
        private readonly IRepository<Franchise> _franchiseRepository;

        [BindProperty]
        public GameFormViewModel GameForm { get; set; } = new();

        public EditModel(
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

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (!_currentUser.IsAuthenticated)
            {
                TempData["ErrorMessage"] = "Please login to edit games.";
                return RedirectToPage("/Account/Login", new { returnUrl = "/Games/Edit" });
            }

            var game = await _gameService.GetGameByIdAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            // Convert GameDto to GameFormViewModel
            GameForm.Id = game.Id;
            GameForm.Title = game.Title;
            GameForm.Description = game.Description;
            GameForm.ReleaseYear = game.ReleaseYear;
            GameForm.Developer = game.Developer;
            GameForm.Publisher = game.Publisher;
            GameForm.ImageUrl = game.ImageUrl;
            GameForm.MetacriticScore = game.MetacriticScore;
            GameForm.MetacriticUrl = game.MetacriticUrl;
            GameForm.FranchiseId = await _gameService.GetGameFranchiseIdAsync(id);
            GameForm.SelectedPlatformIds = await _gameService.GetGamePlatformIdsAsync(id);
            GameForm.SelectedGenreIds = await _gameService.GetGameGenreIdsAsync(id);

            await LoadDropdownsAsync();

            MarkSelectedItems();

            return Page();
        }

        private void MarkSelectedItems()
        {
            var selectedFranchise = GameForm.Franchises
                .FirstOrDefault(f => f.Value == GameForm.FranchiseId?.ToString());
            if (selectedFranchise != null)
            {
                selectedFranchise.Selected = true;
            }
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
                MarkSelectedItems();
                return Page();
            }

            try
            {
                // Convert ViewModel to DTO
                var updateGameDto = new UpdateGameDto()
                {
                    Title = GameForm.Title,
                    Description = GameForm.Description,
                    ReleaseYear = GameForm.ReleaseYear,
                    Developer = GameForm.Developer,
                    Publisher = GameForm.Publisher,
                    ImageUrl = GameForm.ImageUrl,
                    MetacriticScore = GameForm.MetacriticScore,
                    MetacriticUrl = GameForm.MetacriticUrl,
                    FranchiseId = GameForm.FranchiseId,
                    PlatformIds = GameForm.SelectedPlatformIds,
                    GenreIds = GameForm.SelectedGenreIds
                };

                var updatedGame = await _gameService.UpdateGameAsync(GameForm.Id, updateGameDto);

                if (updatedGame == null)
                {
                    ModelState.AddModelError(string.Empty, "Game not found.");
                    await LoadDropdownsAsync();
                    MarkSelectedItems();
                    return Page();
                }

                TempData["SuccessMessage"] = $"Game '{updatedGame.Title}' has been updated successfully.";
                return RedirectToPage("Details", new { id = GameForm.Id });
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await LoadDropdownsAsync();
                MarkSelectedItems();
                return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while updating the game. Please try again");
                await LoadDropdownsAsync();
                MarkSelectedItems();
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

            GameForm.Franchises.Insert(0, new SelectListItem
            {
                Value = "",
                Text = "-- Select Franchise (Optional) --"
            });

            var platforms = await _platformRepository.GetAllAsync();
            GameForm.Platforms = platforms
                .Where(p => !p.IsDeleted)
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                })
                .OrderBy(p => p.Text)
                .ToList();

            var genres = await _genreRepository.GetAllAsync();
            GameForm.Genres = genres
                .Where(g => !g.IsDeleted)
                .Select(g => new SelectListItem
                {
                    Value = g.Id.ToString(),
                    Text = g.Name
                })
                .OrderBy(g => g.Text)
                .ToList();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            if (!_currentUser.IsAuthenticated)
            {
                return Unauthorized();
            }

            if (_currentUser.IsInRole("Admin"))
            {
                try
                {
                    var result = await _gameService.DeleteGameAsync(id);

                    if (result)
                    {
                        TempData["SuccessMessage"] = "Game deleted successfully!";
                        return RedirectToPage("/Games/Index");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Failed to delete game.";
                        return RedirectToPage(new { id });
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Error: {ex.Message}";
                    return RedirectToPage(new { id });
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Please login with an admin role to delete game.";
                return RedirectToPage(new { id });
            }
        }
    }
}
