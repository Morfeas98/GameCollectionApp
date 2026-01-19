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
    }
}
