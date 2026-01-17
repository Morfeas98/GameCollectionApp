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
    public class CollectionRepository : Repository<GameCollection.Domain.Entities.GameCollection>, ICollectionRepository
    {
        public CollectionRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<GameCollection.Domain.Entities.GameCollection>> GetUserCollectionsAsync(int userId)
        {
            return await _context.GameCollections
                .Include(c => c.CollectionGames)
                    .ThenInclude(cg => cg.Game)
                .Where(c => c.UserId == userId && !c.IsDeleted)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<GameCollection.Domain.Entities.GameCollection?> GetCollectionWithGamesAsync(int collectionId)
        {
            return await _context.GameCollections
                .Include(c => c.CollectionGames)
                    .ThenInclude(cg => cg.Game)
                        .ThenInclude(g => g.GamePlatforms)
                            .ThenInclude(gp => gp.Platform)
                .Include(c => c.CollectionGames)
                    .ThenInclude(cg => cg.Game)
                        .ThenInclude(g => g.GameGenres)
                            .ThenInclude(gg => gg.Genre)
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == collectionId && !c.IsDeleted);
        }

        public async Task<bool> CollectionBelongsToUserAsync(int collectionId, int userId)
        {
            return await _context.GameCollections
                .AnyAsync(c => c.Id == collectionId && c.UserId == userId && !c.IsDeleted);
        }

        public async Task AddGameToCollectionAsync(int collectionId, int gameId, int? personalRating, string? personalNotes)
        {
            // Check if Existing
            var existing = await _context.CollectionGames
                .FirstOrDefaultAsync(cg => cg.CollectionId == collectionId && cg.GameId == gameId && !cg.IsDeleted);

            if (existing != null)
                throw new InvalidOperationException("Game is already in collection");

            var collectionGame = new CollectionGame()
            {
                CollectionId = collectionId,
                GameId = gameId,
                DateAdded = DateTime.UtcNow,
                PersonalRating = personalRating,
                PersonalNotes = personalNotes,
                CreatedAt = DateTime.UtcNow
            };

            await _context.CollectionGames.AddAsync(collectionGame);
        }

        public async Task RemoveGameFromCollectionAsync(int collectionId, int gameId)
        {
            var collectionGame = await _context.CollectionGames
                .FirstOrDefaultAsync(cg => cg.CollectionId == collectionId && cg.GameId == gameId && !cg.IsDeleted);

            if (collectionGame != null)
            {
                collectionGame.IsDeleted = true;
                collectionGame.UpdateAt = DateTime.UtcNow;
                _context.CollectionGames.Update(collectionGame);
            }
        }

        public async Task<IEnumerable<Game>> GetCollectionGamesAsync(int collectionId)
        {
            return await _context.CollectionGames
                .Where(cg => cg.CollectionId == collectionId && !cg.IsDeleted)
                .Include(cg => cg.Game)
                    .ThenInclude(g => g.GamePlatforms)
                        .ThenInclude(gp => gp.Platform)
                .Include(cg => cg.Game)
                    .ThenInclude(g => g.GameGenres)
                        .ThenInclude(gg => gg.Genre)
                .Select(cg => cg.Game)
                .ToListAsync();
        }

        public async Task<CollectionGame?> GetCollectionGameAsync(int collectionId, int gameId)
        {
            return await _context.CollectionGames
                .Include(cg => cg.Game)
                .FirstOrDefaultAsync(cg => cg.CollectionId == collectionId && cg.GameId == gameId && !cg.IsDeleted);
        }

        public async Task UpdateCollectionGameAsync(CollectionGame collectionGame)
        {
            collectionGame.UpdateAt = DateTime.UtcNow;
            _context.CollectionGames.Update(collectionGame);
        }
    }
}
