using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GameCollection.Application.Services;
using GameCollection.Application.DTOs;

namespace GameCollection.API.Pages.Games
{
    public class DetailsModel : PageModel
    {
        private readonly IGameService _gameService;
        private readonly ICollectionService _collectionService;

        public GameDto Game { get; set; }
        public IEnumerable<GameDto> Recommendations { get; set; }
        public bool IsInUserCollection { get; set; }
        public int? CollectionGameId { get; set; }
        public int? PersonalRating { get; set; }
        public string? PersonalNotes { get; set; }
        public bool Completed { get; set; }
        public bool CurrentlyPlaying { get; set; }

        public DetailsModel(
            IGameService gameService,
            ICollectionService collectionService)
        {
            _gameService = gameService;
            _collectionService = collectionService;
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

            // TODO: Check if game is in user's collection needs user authentication

            return Page();
        }

        public async Task<IActionResult> OnPostAddToCollectionAsync(int gameId, int collectionId)
        {
            // TODO: Implement add to collection
            return RedirectToPage();
        }
    }
}
