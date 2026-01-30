using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GameCollection.Application.DTOs;
using GameCollection.Domain.Entities;
using GameCollection.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GameCollection.Application.Services
{
    public class CollectionService : ICollectionService
    {
        private readonly ICollectionRepository _collectionRepository;
        private readonly IGameRepository _gameRepository;
        private readonly ICurrentUserService _currentUser;
        private readonly IMapper _mapper;

        public CollectionService(
            ICollectionRepository collectionRepository,
            IGameRepository gameRepository,
            ICurrentUserService currentUser,
            IMapper mapper)
        {
            _collectionRepository = collectionRepository;
            _gameRepository = gameRepository;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CollectionDto>> GetUserCollectionsAsync(int userId)
        {
            var collections = await _collectionRepository.GetUserCollectionsAsync(userId);
            return _mapper.Map<IEnumerable<CollectionDto>>(collections);
        }

        public async Task<CollectionDto?> GetCollectionByIdAsync(int collectionId, int userId)
        {
            // Verify Ownership
            if (!await _collectionRepository.CollectionBelongsToUserAsync(collectionId, userId))
                return null;

            var collection = await _collectionRepository.GetCollectionWithGamesAsync(collectionId);
            return collection != null ? _mapper.Map<CollectionDto>(collection) : null;
        }

        public async Task<CollectionDto> CreateCollectionAsync(CreateCollectionDto collectionDto, int userId)
        {
            var collection = new GameCollection.Domain.Entities.GameCollection
            {
                Name = collectionDto.Name,
                Description = collectionDto.Description,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            var createdCollection = await _collectionRepository.AddAsync(collection);
            await _collectionRepository.SaveChangesAsync();

            return _mapper.Map<CollectionDto>(createdCollection);
        }

        public async Task<CollectionDto?> UpdateCollectionAsync(int collectionId, UpdateCollectionDto collectionDto, int userId)
        {
            // Verify Ownership
            if (!await _collectionRepository.CollectionBelongsToUserAsync(collectionId, userId))
                return null;

            var collection = await _collectionRepository.GetByIdAsync(collectionId);
            if (collection == null)
                return null;

            // Update Fields if Available
            if (!string.IsNullOrWhiteSpace(collectionDto.Name))
                collection.Name = collectionDto.Name;

            if (collectionDto.Description != null)
                collection.Description = collectionDto.Description;

            await _collectionRepository.UpdateAsync(collection);
            await _collectionRepository.SaveChangesAsync();

            return _mapper.Map<CollectionDto>(collection);
        }

        public async Task<bool> DeleteCollectionAsync(int collectionId, int userId)
        {
            // Verify Ownership
            if (!await _collectionRepository.CollectionBelongsToUserAsync(collectionId, userId))
                return false;

            var collection = await _collectionRepository.GetByIdAsync(collectionId);
            if (collection == null)
                return false;

            await _collectionRepository.DeleteAsync(collection);
            await _collectionRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AddGameToCollectionAsync(int collectionId, AddGameToCollectionDto addGameDto, int userId)
        {
         
            // Verify Ownership
            if (!await _collectionRepository.CollectionBelongsToUserAsync(collectionId, userId))
                return false;

            // Verify Game Exists
            var game = await _gameRepository.GetByIdAsync(addGameDto.GameId);
            if (game == null)
                return false;

            try
            {
                await _collectionRepository.AddGameToCollectionAsync(
                    collectionId,
                    addGameDto.GameId,
                    addGameDto.PersonalRating,
                    addGameDto.PersonalNotes,
                    addGameDto.Completed,
                    addGameDto.CurrentlyPlaying);

                await _collectionRepository.SaveChangesAsync();
                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public async Task<bool> RemoveGameFromCollectionAsync(int collectionId, int gameId, int userId)
        {
            // Verify Ownership
            if (!await _collectionRepository.CollectionBelongsToUserAsync(collectionId, userId))
                return false;

            await _collectionRepository.RemoveGameFromCollectionAsync(collectionId, gameId);
            await _collectionRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateGameInCollectionAsync(int collectionId, int gameId, AddGameToCollectionDto updateDto, int userId)
        {
            // Verify Ownership
            if (!await _collectionRepository.CollectionBelongsToUserAsync(collectionId, userId))
                return false;

            var collectionGame = await _collectionRepository.GetCollectionGameAsync(collectionId, gameId);
            if (collectionGame == null)
                return false;

            // Update Fields if Provided
            if (updateDto.PersonalRating.HasValue)
                collectionGame.PersonalRating = updateDto.PersonalRating;

            if (updateDto.PersonalNotes != null)
                collectionGame.PersonalNotes = updateDto.PersonalNotes;

            collectionGame.Completed = updateDto.Completed;

            collectionGame.CurrentlyPlaying = updateDto.CurrentlyPlaying;

            await _collectionRepository.UpdateCollectionGameAsync(collectionGame);
            await _collectionRepository.SaveChangesAsync();

            return true;
        }

        public async Task<CollectionGameDto?> GetUserGameAsync(int userId, int gameId)
        {
            var collectionGame = await _collectionRepository.GetUserGameAsync(userId, gameId);

            if (collectionGame == null)
                return null;

            return _mapper.Map<CollectionGameDto>(collectionGame);
        }

        public async Task<IEnumerable<CollectionDto>> GetCollectionsContainingGameAsync(int userId, int gameId)
        {
            var collections = await _collectionRepository.GetCollectionsContainingGameAsync(userId, gameId);

            return _mapper.Map<IEnumerable<CollectionDto>>(collections);
        }

        public async Task<bool> IsGameInUserCollectionAsync(int userId, int gameId)
        {
            return await _collectionRepository.IsGameInUserCollectionAsync(userId, gameId);
        }

        public async Task<CollectionGameDto?> GetCollectionGameDetailsAsync(int userId, int gameId, int collectionId)
        {
            // Verify ownership
            if (!await _collectionRepository.CollectionBelongsToUserAsync(collectionId, userId))
                return null;

            var collectionGame = await _collectionRepository.GetCollectionGameAsync(collectionId, gameId);
            return collectionGame != null ? _mapper.Map<CollectionGameDto>(collectionGame) : null;
        }
    }
}
