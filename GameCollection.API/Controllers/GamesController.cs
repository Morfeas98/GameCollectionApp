using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GameCollection.Infrastructure.Data;
using GameCollection.Domain.Common;
using GameCollection.Domain.Entities;

namespace GameCollection.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GamesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/games
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> GetGames()
        {
            return await _context.Games
                .Include(g => g.Franchise)
                .Include(g => g.GamePlatforms)
                    .ThenInclude(gp => gp.Platform)
                .Include(g => g.GameGenres)
                    .ThenInclude(gg => gg.Genre)
                .Where(g => !g.IsDeleted)
                .Take(10)
                .ToListAsync()
                ;
        }

        // GET: api/games (by id)
        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGame(int id)
        {
            var game = await _context.Games
                .Include(g => g.Franchise)
                .Include(g => g.GamePlatforms)
                    .ThenInclude(gp => gp.Platform)
                .Include(g => g.GameGenres)
                    .ThenInclude(gg => gg.Genre)
                .FirstOrDefaultAsync(g => g.Id == id && !g.IsDeleted);

            if (game == null)
            {
                return NotFound();
            }

            return game;
        }

        // POST: api/games
        [HttpPost]
        public async Task<ActionResult<Game>> PostGame(Game game)
        {
            // Basic Validation
            if (string.IsNullOrWhiteSpace(game.Title))
            {
                return BadRequest("Title is required");
            }

            if (game.ReleaseYear < 1950 || game.ReleaseYear > DateTime.UtcNow.Year + 2)
            {
                return BadRequest($"Release year must be between 1950 and {DateTime.UtcNow.Year + 2}");
            }

            _context.Games.Add(game);

            return CreatedAtAction(nameof(GetGame), new { id = game.Id }, game);
        }

        // GET: api/games/search?title
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Game>>> SearchGames([FromQuery] string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return BadRequest("Search term is required");
            }

            var games = await _context.Games
                .Where(g => g.Title.Contains(title) && !g.IsDeleted)
                .Take(10)
                .ToListAsync();

            return games;
        }
    }
}
