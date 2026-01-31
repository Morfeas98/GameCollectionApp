using GameCollection.Application.DTOs;
using GameCollection.Application.Services;
using GameCollection.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GameCollection.API.Pages.Games
{
    public class DetailsModel : PageModel
    {
        private readonly IGameService _gameService;
        private readonly ICollectionService _collectionService;
        private readonly ICurrentUserService _currentUser;

        public GameDto Game { get; set; }
        public IEnumerable<GameDto> Recommendations { get; set; }
        public IEnumerable<CollectionDto> UserCollections { get; set; }
        public IEnumerable<CollectionDto> CollectionsContainingGame { get; set; } = new List<CollectionDto>();
        public bool IsInUserCollection { get; set; }
        public int? CollectionGameId { get; set; }
        public int? PersonalRating { get; set; }
        public string? PersonalNotes { get; set; }
        public bool Completed { get; set; }
        public bool CurrentlyPlaying { get; set; }

        public DetailsModel(
            IGameService gameService,
            ICollectionService collectionService,
            ICurrentUserService currentUser)
        {
            _gameService = gameService;
            _collectionService = collectionService;
            _currentUser = currentUser;
            UserCollections = new  List<CollectionDto>();
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Game = await _gameService.GetGameByIdAsync(id);

            if (Game == null)
            {
                return NotFound();
            }

            // Get Recommendations
            Recommendations = await _gameService.GetRecommendationsAsync(id);

            // Get User Collections
            if (_currentUser.IsAuthenticated)
            {
                var userId = _currentUser.GetRequiredUserId();

                UserCollections = await _collectionService.GetUserCollectionsAsync(userId);

                var userGame = await _collectionService.GetUserGameAsync(userId, id);

                if (userGame != null)
                {
                    IsInUserCollection = true;
                    PersonalRating = userGame.PersonalRating;
                    PersonalNotes = userGame.PersonalNotes;
                    Completed = userGame.Completed;
                    CurrentlyPlaying = userGame.CurrentlyPlaying;
                }

                CollectionsContainingGame = await _collectionService.GetCollectionsContainingGameAsync(userId, id);
            }

            return Page();
        }

        public async Task<IActionResult> OnPostQuickAddAsync(
            int gameId,
            int collectionId,
            int personalRating = 5,
            string personalNotes = "",
            bool completed = false,
            bool currentlyPlaying = false)
        {
            if (!_currentUser.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login", new
                {
                    returnUrl = $"/Games/Details/{gameId}",
                    errorMessage = "Please login to perform this action"
                });
            }

            try
            {
                var userId = _currentUser.GetRequiredUserId();


                var addGameDto = new AddGameToCollectionDto
                {
                    GameId = gameId,
                    PersonalRating = personalRating,
                    PersonalNotes = personalNotes,
                    Completed = completed,
                    CurrentlyPlaying = currentlyPlaying
                };

                var result = await _collectionService.AddGameToCollectionAsync(
                    collectionId, addGameDto, userId);

                if (result)
                {
                    
                    TempData["SuccessMessage"] = "Game added to collection!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to add game to collection.";
                }

                return RedirectToPage(new { id = gameId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
                return RedirectToPage(new { id = gameId });
            }
        }

        public async Task<IActionResult> OnPostRemoveFromCollectionAsync(int gameId, int? collectionId = null)
        {
            if (!_currentUser.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login", new
                {
                    returnUrl = $"/Games/Details/{gameId}",
                    errorMessage = "Please login to modify collections"
                });
            }

            try
            {
                var userId = _currentUser.GetRequiredUserId();

                if (!collectionId.HasValue)
                {
                    var collections = await _collectionService.GetCollectionsContainingGameAsync(userId, gameId);

                    bool removedFromAny = false;

                    foreach (var collection in collections)
                    {
                        var result = await _collectionService.RemoveGameFromCollectionAsync(collection.Id, gameId, userId);

                        if (result) removedFromAny = true;
                    }

                    if (removedFromAny)
                    {
                        TempData["SuccessMessage"] = "Game removed from all collections!";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Game not found in any collection";
                    }
                }
                else
                {
                    var result = await _collectionService.RemoveGameFromCollectionAsync(collectionId.Value, gameId, userId);

                    if (result)
                    {
                        TempData["SuccessMessage"] = "Game removed from collection successfully!";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Failed to remove game from collection";
                    }
                }

                return RedirectToPage(new { id = gameId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
                return RedirectToPage(new { id = gameId });
            }
        }

        public async Task<IActionResult> OnPostUpdateNotesAsync(
            int gameId,
            int collectionId,
            int personalRating,
            string personalNotes,
            bool completed = false,
            bool currentlyPlaying = false)
        {
            if (!_currentUser.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login", new
                {
                    returnUrl = $"/Games/Details/{gameId}",
                    errorMessage = "Please login to update notes"
                });
            }

            try
            {
                var userId = _currentUser.GetRequiredUserId();

                var updateDto = new AddGameToCollectionDto()
                {
                    GameId = gameId,
                    PersonalRating = personalRating,
                    PersonalNotes = personalNotes,
                    Completed = completed,
                    CurrentlyPlaying = currentlyPlaying
                };

                var result = await _collectionService.UpdateGameInCollectionAsync(collectionId, gameId, updateDto, userId);

                if (result)
                {
                    TempData["SuccessMessage"] = "Notes updated successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to update notes.";
                }

                return RedirectToPage(new { id = gameId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
                return RedirectToPage(new { id = gameId });
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            if (!_currentUser.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login", new
                {
                    returnUrl = $"/Games/Details/{id}",
                    errorMessage = "Please login with an admin role to delete game."
                });
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
