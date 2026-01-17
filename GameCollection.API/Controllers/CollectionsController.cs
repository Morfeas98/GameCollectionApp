using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GameCollection.Application.DTOs;
using GameCollection.Application.Services;
using System.Security.Claims;

namespace GameCollection.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CollectionsController : ControllerBase
    {
        private readonly ICollectionService _collectionService;

        public CollectionsController(ICollectionService collectionService)
        {
            _collectionService = collectionService;
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                throw new UnauthorizedAccessException("User not authenticated");

            return int.Parse(userIdClaim);
        }

        // Get: api/collections
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CollectionDto>>> GetUserCollections()
        {
            try
            {
                var userId = GetCurrentUserId();
                var collections = await _collectionService.GetUserCollectionsAsync(userId);
                return Ok(collections);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
        }

        // Get: api/collections/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CollectionDto>> GetCollection(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                var collection = await _collectionService.GetCollectionByIdAsync(id, userId);

                if (collection == null)
                    return NotFound(new { error = "Collection not found or access denied" });

                return Ok(collection);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
        }

        // POST: api/collections
        [HttpPost]
        public async Task<ActionResult<CollectionDto>> CreateCollection(CreateCollectionDto collectionDto)
        {
            try
            {
                var userId = GetCurrentUserId();
                var collection = await _collectionService.CreateCollectionAsync(collectionDto, userId);

                return CreatedAtAction(nameof(GetCollection), new { id = collection.Id }, collection);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occured while creating collection" });
            }
        }

        // PUT: api/collections/5
        [HttpPost("{id}")]
        public async Task<ActionResult<CollectionDto>> UpdateCollection(int id, UpdateCollectionDto collectionDto)
        {
            try
            {
                var userId = GetCurrentUserId();
                var updatedCollection = await _collectionService.UpdateCollectionAsync(id, collectionDto, userId);

                if (updatedCollection == null)
                    return NotFound(new { error = "Collection not found or access denied" });

                return Ok(updatedCollection);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
        }

        // DELETE: api/collections/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCollection(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                var result = await _collectionService.DeleteCollectionAsync(id, userId);

                if (!result)
                    return NotFound(new { error = "Collection not found or access denied" });

                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
        }

        // POST: api/collections/5/games
        [HttpPost("{collectionId}/games")]
        public async Task<IActionResult> AddGameToCollection(int collectionId, AddGameToCollectionDto addGameDto)
        {
            try
            {
                var userId = GetCurrentUserId();
                var result = await _collectionService.AddGameToCollectionAsync(collectionId, addGameDto, userId);

                if (!result)
                    return BadRequest(new { error = "Failed to add game to collection" });

                return Ok(new { message = "Game added to collection successfully" });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
        }

        // DELETE: api/collections/5/games/10
        [HttpDelete("{collectionId}/games/{gameId}")]
        public async Task<IActionResult> RemoveGameFromCollection(int collectionId, int gameId)
        {
            try
            {
                var userId = GetCurrentUserId();
                var result = await _collectionService.RemoveGameFromCollectionAsync(collectionId, gameId, userId);

                if (!result)
                    return BadRequest(new { error = "Failed to remove game from collection" });

                return Ok(new { message = "Game removed from collection successfully" });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
        }

        // PUT: api/collection/5/games/10
        [HttpPut("{collectioId}/games/{gameId}")]
        public async Task<IActionResult> UpdateGameInCollection(int collectionId, int gameId, AddGameToCollectionDto updateDto)
        {
            try
            {
                var userId = GetCurrentUserId();
                var result = await _collectionService.UpdateGameInCollectionAsync(collectionId, gameId, updateDto, userId);

                if (!result)
                    return BadRequest(new { error = "Failed to update game in collection" });

                return Ok(new { message = "Game updated in collection successfully" });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
        }
    }
}
