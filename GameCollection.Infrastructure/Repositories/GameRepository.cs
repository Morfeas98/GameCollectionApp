using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GameCollection.Domain.Entities;
using GameCollection.Domain.Repositories;
using GameCollection.Infrastructure.Data;
using Microsoft.IdentityModel.Tokens;

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
                    g.Publisher.Contains(searchTerm) ||
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

        public async Task<(IEnumerable<Game> Games, int TotalCount)> GetFilteredGamesAsync(
            string? searchTerm = null,
            int? platformId = null,
            int? genreId = null,
            int? franchiseId = null,
            int? minYear = null,
            int? maxYear = null,
            string sortBy = "title_asc",
            int pageNumber = 1,
            int pageSize = 12,
            bool includeDeleted = false)
        {
            // Base Query
            var query = _context.Games
                .Include(g => g.Franchise)
                .Include(g => g.GamePlatforms)
                    .ThenInclude(gp => gp.Platform)
                .Include(g => g.GameGenres)
                    .ThenInclude(gg => gg.Genre)
                .AsQueryable();

            // Exclude Deleted unless specified
            if (!includeDeleted)
            {
                query = query.Where(g => !g.IsDeleted);
            }
            
            // Search Filter
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var term = searchTerm.Trim().ToLower(); 
                query = query.Where(g =>
                    g.Title.ToLower().Contains(term) ||
                    (g.Developer != null && g.Developer.ToLower().Contains(term)) ||
                    (g.Publisher != null && g.Publisher.ToLower().Contains(term)) ||
                    (g.Franchise != null && g.Franchise.Name.ToLower().Contains(term)) ||
                    (g.Description != null && g.Description.ToLower().Contains(term)));
            }

            // Platform Filter
            if (platformId.HasValue && platformId > 0)
            {
                query = query
                    .Where(g => g.GamePlatforms.Any(gp =>
                                    gp.PlatformId == platformId && !gp.Platform.IsDeleted));
            }

            // Genre Filter
            if (genreId.HasValue && genreId > 0)
            {
                query = query
                    .Where(g => g.GameGenres.Any(gg =>
                                    gg.GenreId == genreId && !gg.Genre.IsDeleted));
            }

            // Franchise filter
            if (franchiseId.HasValue && franchiseId > 0)
            {
                query = query.Where(g =>
                    g.FranchiseId.Value == franchiseId && !g.Franchise.IsDeleted);
            }

            // Year Range Filter
            if (minYear.HasValue)
            {
                query = query
                    .Where(g => g.ReleaseYear >= minYear.Value);
            }
            if (maxYear.HasValue)
            {
                query = query
                    .Where(g => g.ReleaseYear <= maxYear.Value);
            }

            // Sorting
            query = sortBy?.ToLower() switch
            {
                "title_desc" => query.OrderByDescending(g => g.Title),
                "year_asc" => query.OrderBy(g => g.ReleaseYear),
                "year_desc" => query.OrderByDescending(g => g.ReleaseYear),
                "rating_asc" => query.OrderBy(g => g.MetacriticScore ?? 0),
                "rating_desc" => query.OrderByDescending(g => g.MetacriticScore ?? 0),
                _ => query.OrderBy(g => g.Title)    // Default: title_asc
            };

            // Total Count before Pagination
            var totalCount = await query.CountAsync();

            // Pagination
            var games = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (games, totalCount);
        }
    }
}
