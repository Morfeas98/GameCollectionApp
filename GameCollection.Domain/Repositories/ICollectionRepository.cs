using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameCollection.Domain.Common;
using GameCollection.Domain.Entities;

namespace GameCollection.Domain.Repositories
{
    public interface ICollectionRepository : IRepository<GameCollection.Domain.Entities.GameCollection>
    {
        Task<IEnumerable<GameCollection.Domain.Entities.GameCollection>> GetUserCollectionsAsync(int userId);
        Task<GameCollection.Domain.Entities.GameCollection> GetCollectionWithGamesAsync(int collectionId);
        Task<bool> CollectionBelongsToUserAsync(int collectionId, int userID);
        Task AddGameToCollectionAsync(int collectionId, int gameId, int? personalRating, string? personalNotes);
        Task RemoveGameFromCollectionAsync(int collectionId, int gameId);
        Task<IEnumerable<Game>> GetCollectionGamesAsync(int collectionId);
        Task<CollectionGame> GetCollectionGameAsync(int collectionId, int gameId);
        Task UpdateCollectionGameAsync(CollectionGame collectionGame);
        Task<int> GetUserCollectionCountAsync(int userId);
        Task<int> GetUserTotalGamesInCollectionsAsync(int userId);
        Task<int> GetUserCompletedGamesCountAsync(int userId);
        Task<int> GetUserCurrentlyPlayingCountAsync(int userId);
        Task<double> GetUserAverageRatingAsync(int userId);
        Task<DateTime?> GetUserLastActivityAsync(int userId);
    }
}
