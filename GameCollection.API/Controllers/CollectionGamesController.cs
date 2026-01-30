using GameCollection.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameCollection.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CollectionGamesController : ControllerBase
    {
        private readonly ICollectionService _collectionService;
        private readonly ICurrentUserService _currentUser;

        public CollectionGamesController(
            ICollectionService collectionService, 
            ICurrentUserService currentUser)
        {
            _collectionService = collectionService;
            _currentUser = currentUser;
        }
        [HttpGet("details/{gameId}/{collectionId}")]
        public async Task<IActionResult> GetCollectionGameDetails(int gameId, int collectionId)
        {
            if (!_currentUser.IsAuthenticated)
                return Unauthorized();

            var userId = _currentUser.GetRequiredUserId();
            var details = await _collectionService.GetCollectionGameDetailsAsync(userId, gameId, collectionId);

            if (details == null)
                return NotFound();

            return Ok(details);
        }
    }
}