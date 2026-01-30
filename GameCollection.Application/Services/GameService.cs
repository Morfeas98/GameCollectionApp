using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GameCollection.Application.DTOs;
using GameCollection.Domain.Common;
using GameCollection.Domain.Entities;
using GameCollection.Domain.Repositories;

namespace GameCollection.Application.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;
        private readonly IRepository<Platform> _platformRepository;
        private readonly IRepository<Genre> _genreRepository;
        private readonly IRepository<Franchise> _franchiseRepository;
        private readonly IMapper _mapper;


        public GameService(
            IGameRepository gameRepository,
            IRepository<Platform> platformRepository,
            IRepository<Genre> genreRepository,
            IRepository<Franchise> franchiseRepository,
            IMapper mapper)
        {
            _gameRepository = gameRepository;
            _platformRepository = platformRepository;
            _genreRepository = genreRepository;
            _franchiseRepository = franchiseRepository;
            _mapper = mapper;
        }


        public async Task<IEnumerable<GameDto>> GetAllGamesAsync()
        {
            var games = await _gameRepository.GetGamesWithDetailsAsync();
            return _mapper.Map<IEnumerable<GameDto>>(games);
        }


        public async Task<GameDto?> GetGameByIdAsync(int id)
        {
            var game = await _gameRepository.GetGameWithDetailsAsync(id);
            return game != null ? _mapper.Map<GameDto>(game) : null;
        }


        public async Task<GameDto> CreateGameAsync(CreateGameDto gameDto)
        {
            // Validate Required Fields
            if (string.IsNullOrWhiteSpace(gameDto.Title))
                throw new ArgumentException("Title is required");

            if (gameDto.ReleaseYear < 1950 || gameDto.ReleaseYear > DateTime.UtcNow.Year + 2)
                throw new ArgumentException($"Release year must be between 1950 and {DateTime.UtcNow.Year + 2}");

            var game = _mapper.Map<Game>(gameDto);
            game.CreatedAt = DateTime.UtcNow;

            // Add Platforms
            foreach (var platformId in gameDto.PlatformIds)
            {
                var platform = await _platformRepository.GetByIdAsync(platformId);
                if (platform == null)
                    throw new ArgumentException($"Platform with ID {platformId} not found");

                game.GamePlatforms.Add(new GamePlatform { Platform = platform });
            }

            // Add Genres
            foreach (var genreId in gameDto.GenreIds)
            {
                var genre = await _genreRepository.GetByIdAsync(genreId);
                if (genre == null)
                    throw new ArgumentException($"Genre with ID {genreId} not found");

                game.GameGenres.Add(new GameGenre { Genre = genre });
            }

            // Set Franchise
            if (gameDto.FranchiseId.HasValue)
            {
                var franchise = await _franchiseRepository.GetByIdAsync(gameDto.FranchiseId.Value);
                if (franchise == null)
                    throw new ArgumentException($"Franchise with ID {gameDto.FranchiseId} not found");

                game.Franchise = franchise;
            }

            // Set Image URL
            if (!string.IsNullOrEmpty(gameDto.ImageUrl))
                game.ImageUrl = gameDto.ImageUrl;

            // Set Metacritic Score
            if (gameDto.MetacriticScore.HasValue)
                game.MetacriticScore = gameDto.MetacriticScore;

            // Set Metacritic URL
            if (!string.IsNullOrEmpty(gameDto.MetacriticUrl))
                game.MetacriticUrl = gameDto.MetacriticUrl;

            var createdGame = await _gameRepository.AddAsync(game);
            await _gameRepository.SaveChangesAsync();

            return _mapper.Map<GameDto>(createdGame);
        }


        public async Task<IEnumerable<GameDto>> SearchGamesAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return Enumerable.Empty<GameDto>();

            var games = await _gameRepository.SearchGamesAsync(searchTerm);
            return _mapper.Map<IEnumerable<GameDto>>(games);
        }


        public async Task<IEnumerable<GameDto>> GetRecommendationsAsync(int gameId)
        {
            var baseGame = await _gameRepository.GetGameWithDetailsAsync(gameId);
            if (baseGame == null)
                return Enumerable.Empty<GameDto>();

            var recommendations = new List<Game>();

            // 1. Same Franchise
            if (baseGame.FranchiseId.HasValue)
            {
                var franchiseGames = await _gameRepository.GetGamesByFranchiseAsync(baseGame.FranchiseId.Value);
                recommendations.AddRange(franchiseGames.Where(g => g.Id != gameId));
            }

            // 2. Same Genres
            var genreIds = baseGame.GameGenres.Select(gg => gg.GenreId).ToList();
            foreach (var genreId in genreIds)
            {
                var genreGames = await _gameRepository.GetGamesByGenreAsync(genreId);
                recommendations.AddRange(genreGames.Where(g => g.Id != gameId && !recommendations.Any(r => r.Id == g.Id)));
            }

            // 3. Similar Metacritic Score (±10 points)
            if (baseGame.MetacriticScore.HasValue)
            {
                var scoreGames = (await _gameRepository.GetAllAsync())
                    .Where(g => g.MetacriticScore.HasValue &&
                                    Math.Abs(g.MetacriticScore.Value - baseGame.MetacriticScore.Value) <= 10 &&
                                    g.Id != gameId &&
                                    !recommendations.Any(r => r.Id == g.Id));
                recommendations.AddRange(scoreGames);
            }

            return _mapper.Map<IEnumerable<GameDto>>(recommendations.Take(10));
        }


        public async Task<GameDto?> UpdateGameAsync(int id, UpdateGameDto gameDto)
        {
            var game = await _gameRepository.GetGameWithDetailsAsync(id);
            if (game == null)
                return null;

            // Update Fields if provided
            if (!string.IsNullOrWhiteSpace(gameDto.Title))
                game.Title = gameDto.Title;

            if (gameDto.Description != null)
                game.Description = gameDto.Description;

            if (gameDto.ReleaseYear.HasValue)
                game.ReleaseYear = gameDto.ReleaseYear.Value;

            if (gameDto.Developer != null)
                game.Developer = gameDto.Developer;

            if (gameDto.Publisher != null)
                game.Publisher = gameDto.Publisher;

            if (gameDto.ImageUrl != null)
                game.ImageUrl = gameDto.ImageUrl;

            if (gameDto.MetacriticScore.HasValue)
                game.MetacriticScore = gameDto.MetacriticScore;

            if (gameDto.MetacriticUrl != null)
                game.MetacriticUrl = gameDto.MetacriticUrl;

            // Update Franchise if Provided
            if (gameDto.FranchiseId.HasValue)
            {
                var franchise = await _franchiseRepository.GetByIdAsync(gameDto.FranchiseId.Value);
                if (franchise != null)
                    game.Franchise = franchise;
            }
            else if (gameDto.FranchiseId == 0)
            {
                game.FranchiseId = null;
                game.Franchise = null;
            }

            // Update Platforms if Provided
            if (gameDto.PlatformIds != null)
            {
                game.GamePlatforms.Clear();

                foreach (var platformId in gameDto.PlatformIds)
                {
                    var platform = await _platformRepository.GetByIdAsync(platformId);
                    if (platform == null)
                        throw new ArgumentException($"Platform with ID {platformId} not found");

                    game.GamePlatforms.Add(new GamePlatform { Platform = platform });
                }
            }

            // Update Genres if Provided
            if (gameDto.GenreIds != null)
            {
                game.GameGenres.Clear();

                foreach (var genreId in gameDto.GenreIds)
                {
                    var genre = await _genreRepository.GetByIdAsync(genreId);
                    if (genre == null)
                        throw new ArgumentException($"Genre with ID {genreId} not found");

                    game.GameGenres.Add(new GameGenre { Genre = genre });
                }
            }

            game.UpdatedAt = DateTime.UtcNow;

            await _gameRepository.UpdateAsync(game);
            await _gameRepository.SaveChangesAsync();

            //return await GetGameByIdAsync(id);
            return _mapper.Map<GameDto>(game);
        }


        public async Task<bool> DeleteGameAsync(int id)
        {
            var game = await _gameRepository.GetGameWithDetailsAsync(id);
            if (game == null)
                return false;

            game.IsDeleted = true;
            game.UpdatedAt = DateTime.UtcNow;

            foreach (var gamePlatform in game.GamePlatforms)
            {
                gamePlatform.IsDeleted = true;
                gamePlatform.UpdatedAt = DateTime.UtcNow;
            }

            foreach (var gameGenre in game.GameGenres)
            {
                gameGenre.IsDeleted = true;
                gameGenre.UpdatedAt = DateTime.UtcNow;
            }

            await _gameRepository.SaveChangesAsync();

            return true;
        }

        public async Task<List<int>> GetGamePlatformIdsAsync(int gameId)
        {
            var game = await _gameRepository.GetGameWithDetailsAsync(gameId);
            if (game == null)
                return new List<int>();

            return game.GamePlatforms
                .Select(gp => gp.PlatformId)
                .ToList();
        }


        public async Task<List<int>> GetGameGenreIdsAsync(int gameId)
        {
            var game = await _gameRepository.GetGameWithDetailsAsync(gameId);
            if (game == null)
                return new List<int>();

            return game.GameGenres
                .Select(gg => gg.GenreId)
                .ToList();
        }


        public async Task<int?> GetGameFranchiseIdAsync(int gameId)
        {
            var game = await _gameRepository.GetGameWithDetailsAsync(gameId);
            return game?.FranchiseId;
        }

        public async Task<List<GameDto>> GetRecentTopRatedGamesAsync(int count)
        {
            var currentYear = DateTime.Now.Year;
            var games = await _gameRepository.GetAllAsync();

            return games
                .Where(g => g.ReleaseYear == currentYear && g.MetacriticScore.HasValue)
                .OrderByDescending(g => g.MetacriticScore)
                .Take(count)
                .Select(g => _mapper.Map<GameDto>(g))
                .ToList();
        }

        public async Task<List<GameDto>> GetTopRatedGamesAsync(int count)
        {
            var recentTopRated = await GetRecentTopRatedGamesAsync(count);
            var recentTopRatedIds = recentTopRated.Select(g => g.Id).ToHashSet();

            var games = await _gameRepository.GetAllAsync();

            return games
                .Where(g => g.MetacriticScore.HasValue && !recentTopRatedIds.Contains(g.Id))
                .OrderByDescending(g => g.MetacriticScore)
                .Take(count)
                .Select(g => _mapper.Map<GameDto>(g))
                .ToList();
        }

        public async Task<GamePagedResultDto> GetFilteredGamesAsync(GameQueryParams queryParams)
        {
            // Validate Pagination
            if (queryParams.PageNumber < 1) queryParams.PageNumber = 1;
            if (queryParams.PageSize < 1 || queryParams.PageSize > 50) queryParams.PageSize = 12;

            // Get Filtered Games from Repository
            var (games, totalCount) = await _gameRepository.GetFilteredGamesAsync(
                searchTerm: queryParams.SearchQuery,
                platformId: queryParams.PlatformId,
                genreId: queryParams.GenreId,
                franchiseId: queryParams.FranchiseId,
                minYear: queryParams.MinYear,
                maxYear: queryParams.MaxYear,
                sortBy: queryParams.SortBy,
                pageNumber: queryParams.PageNumber,
                pageSize: queryParams.PageSize,
                includeDeleted: queryParams.IncludeDeleted);

            var gameDtos = _mapper.Map<List<GameDto>>(games);

            return new GamePagedResultDto
            {
                Games = gameDtos,
                TotalCount = totalCount,
                PageNumber = queryParams.PageNumber,
                PageSize = queryParams.PageSize
            };
        }

        public async Task<GamePagedResultDto> GetFilteredGamesAsync(
            string? searchQuery = null,
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
            var queryParams = new GameQueryParams
            {
                SearchQuery = searchQuery,
                PlatformId = platformId,
                GenreId = genreId,
                FranchiseId = franchiseId,
                MinYear = minYear,
                MaxYear = maxYear,
                SortBy = sortBy,
                PageNumber = pageNumber,
                PageSize = pageSize,
                IncludeDeleted = includeDeleted
            };

            return await GetFilteredGamesAsync(queryParams);
        }
    }
}
