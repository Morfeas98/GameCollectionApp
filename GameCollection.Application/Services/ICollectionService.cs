using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameCollection.Application.DTOs;

namespace GameCollection.Application.Services
{
    public interface ICollectionService
    {
        Task<IEnumerable<CollectionDto>> GetUserCollectionsAsync(int userId);
        Task<CollectionDto?> GetCollectionByIdAsync(int collectionId, int userId);
        Task<CollectionDto> CreateCollectionAsync(CreateCollectionDto collectionDto, int userId);
        Task<CollectionDto?> UpdateCollectionAsync(int collectionId, UpdateCollectionDto collectionDto, int userId);
        Task<bool> DeleteCollectionAsync(int collectionId, int userId);
        Task<bool> AddGameToCollectionAsync(int collectionId, AddGameToCollectionDto addGameDto, int userId);
        Task<bool> RemoveGameFromCollectionAsync(int collectionId, int gameId, int userId);
        Task<bool> UpdateGameInCollectionAsync(int collectionId, int gameId, AddGameToCollectionDto updateDto, int userId);
    }
}
