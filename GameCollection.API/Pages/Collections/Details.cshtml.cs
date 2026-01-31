using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GameCollection.Application.Services;
using GameCollection.Application.DTOs;
using GameCollection.Infrastructure.Data;
using GameCollection.Domain.Entities;
using System.Collections.ObjectModel;

namespace GameCollection.API.Pages.Collections
{
    public class DetailsModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly ICollectionService _collectionService;
        private readonly IGameService _gameService;
        private readonly ICurrentUserService _currentUser;

        public CollectionDto Collection { get; set; }
        public List<SelectListItem> AllGames { get; set; } = new();
        public bool IsOwner { get; set; }

        [BindProperty]
        public AddGameToCollectionDto AddGameDto { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string SortBy { get; set; } = "date";

        [BindProperty(SupportsGet = true)]
        public string SortOrder { get; set; } = "desc";

        public DetailsModel(
            AppDbContext context,
            ICollectionService collectionService,
            IGameService gameService,
            ICurrentUserService currentUser)
        {
            _context = context;
            _collectionService = collectionService;
            _gameService = gameService;
            _currentUser = currentUser;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (!_currentUser.IsAuthenticated)
            {
                TempData["ErrorMessage"] = "Please login to view collections.";
                return RedirectToPage("/Account/Login", new { returnUrl = $"/Collections/Details/{id}" });
            }

            var userId = _currentUser.GetRequiredUserId();
            Collection = await _collectionService.GetCollectionByIdAsync(id, userId);

            if (Collection == null)
            {
                return NotFound();
            }

            var collectionOwnerId = await _context.GameCollections
                .Where(c => c.Id == id)
                .Select(c => c.UserId)
                .FirstOrDefaultAsync();

            IsOwner = collectionOwnerId == userId;

            if (Collection.Games != null && Collection.Games.Any())
            {
                Collection.Games = ApplySorting(Collection.Games, SortBy, SortOrder).ToList();
            }

            await LoadAllGamesAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAddGameAsync(int id)
        {
            if (!_currentUser.IsAuthenticated)
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                await ReloadPageDataAsync(id);
                return Page();
            }

            try
            {
                var userId = _currentUser.GetRequiredUserId();
                var result = await _collectionService.AddGameToCollectionAsync(id, AddGameDto, userId);

                if (!result)
                {
                    ModelState.AddModelError(string.Empty, "Failed to add game to collection");
                    await ReloadPageDataAsync(id);
                    return Page();
                }

                TempData["SuccessMessage"] = "Game added to collection successfully!";
                return RedirectToPage(new
                {
                    id,
                    sortBy = SortBy,
                    sortOrder = SortOrder
                });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await ReloadPageDataAsync(id);
                return Page();
            }
        }

        public async Task<IActionResult> OnPostRemoveGameAsync(int id, int gameId)
        {
            try
            {
                if (!_currentUser.IsAuthenticated)
                {
                    return Unauthorized();
                }

                var userId = _currentUser.GetRequiredUserId();
                var result = await _collectionService.RemoveGameFromCollectionAsync(id, gameId, userId);

                if (!result)
                {
                    TempData["ErrorMessage"] = "Failed to remove game from collection";
                }
                else
                {
                    TempData["SuccessMessage"] = "Game removed from collection successfully!";
                }

                return RedirectToPage(new
                {
                    id,
                    sortBy = SortBy,
                    sortOrder = SortOrder
                });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToPage(new
                {
                    id,
                    sortBy = SortBy,
                    sortOrder = SortOrder
                });
            }
        }

        private async Task LoadAllGamesAsync()
        {
            var allGames = await _gameService.GetAllGamesAsync();

            var collectionGameIds = Collection?.Games?.Select(g => g.GameId).ToList() ?? new List<int>();

            AllGames = allGames
                .Where(g => !collectionGameIds.Contains(g.Id))
                .Select(g => new SelectListItem()
                {
                    Value = g.Id.ToString(),
                    Text = $"{g.Title} ({g.ReleaseYear})"
                })
                .OrderBy(g => g.Text)
                .ToList();

            if (AllGames.Any())
            {
                AllGames.Insert(0, new SelectListItem()
                {
                    Value = "",
                    Text = "-- Select a game to add --"
                });
            }
        }

        private async Task ReloadPageDataAsync(int id)
        {
            var userId = _currentUser.GetRequiredUserId();
            Collection = await _collectionService.GetCollectionByIdAsync(id, userId);

            if (Collection.Games != null && Collection.Games.Any())
            {
                Collection.Games = ApplySorting(Collection.Games, SortBy, SortOrder).ToList();
            }

            await LoadAllGamesAsync();
        }

        private IEnumerable<CollectionGameDto> ApplySorting(
            IEnumerable<CollectionGameDto> games,
            string sortBy,
            string sortOrder)
        {
            if (string.IsNullOrEmpty(sortBy))
                sortBy = "date";
            if (string.IsNullOrEmpty(sortOrder))
                sortOrder = "desc";

            switch (sortBy.ToLower())
            {
                case "date":
                    return sortOrder == "desc"
                        ? games.OrderByDescending(g => g.DateAdded)
                        : games.OrderBy(g => g.DateAdded);

                case "date_old":
                    return games.OrderBy(g => g.DateAdded);

                case "title":
                    return games.OrderBy(g => g.GameTitle);

                case "rating":
                    return sortOrder == "desc"
                        ? games.OrderByDescending(g => g.PersonalRating ?? 0)
                        : games.OrderBy(g => g.PersonalRating ?? 0);

                default:
                    return games.OrderByDescending(g => g.DateAdded);
            }
        }
    }
}
