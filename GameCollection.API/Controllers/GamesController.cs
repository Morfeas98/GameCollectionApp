using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using GameCollection.Infrastructure.Data;
using GameCollection.Domain.Common;
using GameCollection.Domain.Entities;
using GameCollection.Application.DTOs;
using GameCollection.Application.Services;
using System.Security.Claims;


namespace GameCollection.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class GamesController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GamesController(IGameService gameService)
        {
            _gameService = gameService;
        }

        // GET:api/games
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetGames()
        {
            var games = await _gameService.GetAllGamesAsync();
            return Ok(games);
        }

        // GET: api/games/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GameDto>> GetGame(int id)
        {
            var game = await _gameService.GetGameByIdAsync(id);

            if (game == null)
                return NotFound();

            return Ok(game);
        }

        // POST: api/games
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<GameDto>> CreateGame(CreateGameDto gameDto)
        {
            try
            {
                var createdGame = await _gameService.CreateGameAsync(gameDto);
                return CreatedAtAction(nameof(GetGame), new { id = createdGame.Id }, createdGame);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occured while creating the game" });
            }
        }

        // PUT: api/games/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<GameDto>> UpdateGame(int id, UpdateGameDto gameDto)
        {
            try
            {
                var updatedGame = await _gameService.UpdateGameAsync(id, gameDto);

                if (updatedGame == null)
                    return NotFound();

                return Ok(updatedGame);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // DELETE: api/games/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var result = await _gameService.DeleteGameAsync(id);

            if (!result)
                return NotFound();

            return NoContent();
        }

        // GET: api/games/search?query=zelda
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<GameDto>>> SearchGames([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest(new { error = "Search query is required!" });

            var games = await _gameService.SearchGamesAsync(query);
            return Ok(games);
        }

        // GET: api/games/5/recommendations
        [HttpGet("{id}/recommendations")]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetRecommendations(int id)
        {
            var recommendations = await _gameService.GetRecommendationsAsync(id);
            return Ok(recommendations);
        }

        [AllowAnonymous]
        [HttpGet("public")]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetPublicGames()
        {
            var games = await _gameService.GetAllGamesAsync();
            return Ok(games.Take(5));
        }

        [HttpPost("admin-only")]
        [Authorize(Roles = "Admin")]
        public IActionResult AdminOnlyEndpoint()
        {
            return Ok(new { message = "Welcome Admin!" });
        }

        // Get current User
        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                throw new UnauthorizedAccessException("User not authenticated");

            return int.Parse(userIdClaim);
        }

        [HttpGet("my-recommendations")]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetMyRecommendations()
        {
            try
            {
                var userId = GetCurrentUserId();
                var allGames = await _gameService.GetAllGamesAsync();
                return Ok(allGames.Take(3));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
        }
    }
}
