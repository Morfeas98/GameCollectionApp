using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GameCollection.Domain.Entities;
using GameCollection.Domain.Repositories;
using GameCollection.Infrastructure.Data;

namespace GameCollection.Infrastructure.Repositories
{
    public class GameRepository : Repository<Game>, IGameRepository
    {
        public GameRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Game>> GetGamesWithDetailsAsync()
        {
            return await _context.Games
                .Include(g => g.Franchise)
                .Include(g => g.GamePlatforms)
                    .ThenInclude(gp => gp.Platform)
                .Include(g => g.GameGenres)
                    .ThenInclude(gg => gg.Genre)
                .Where(g => !g.IsDeleted)
                .ToListAsync();
        }

        public async Task<Game?> GetGameWithDetailsAsync(int id)
        {
            return await _context.Games
                .Include(g => g.Franchise)
                .Include(g => g.GamePlatforms)
                    .ThenInclude(gp => gp.Platform)
                .Include(g => g.GameGenres)
                    .ThenInclude(gg => gg.Genre)
                .FirstOrDefaultAsync(g => g.Id == id && !g.IsDeleted);
        }

        public async Task<IEnumerable<Game>> SearchGamesAsync(string searchTerm)
        {
            return await _context.Games
                .Include(g => g.Franchise)
                .Where(g => !g.IsDeleted &&
                    (g.Title.Contains(searchTerm) ||
                    g.Developer.Contains(searchTerm) ||
                    (g.Description != null && g.Description.Contains(searchTerm))))
                .Take(20)
                .ToListAsync();
        }

        public async Task<IEnumerable<Game>> GetGamesByPlatformAsync(int platformId)
        {
            return await _context.Games
                .Include(g => g.Franchise)
                .Where(g => !g.IsDeleted &&
                    g.GamePlatforms.Any(gp => gp.PlatformId == platformId))
                .ToListAsync();
        }

        public async Task<IEnumerable<Game>> GetGamesByGenreAsync(int genreId)
        {
            return await _context.Games
                .Include(g => g.Franchise)
                .Where(g => !g.IsDeleted &&
                    g.GameGenres.Any(gg => gg.GenreId == genreId))
                .ToListAsync();
        }

        public async Task<IEnumerable<Game>> GetGamesByFranchiseAsync(int franchiseId)
        {
            return await _context.Games
                .Include(g => g.Franchise)
                .Where(g => !g.IsDeleted && g.FranchiseId == franchiseId)
                .ToListAsync();
        }
    }
}
