using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using AutoMapper;
using GameCollection.Application.Services;
using GameCollection.Application.DTOs;
using GameCollection.Domain.Entities;
using GameCollection.Domain.Repositories;
using GameCollection.Application.Common;
using GameCollection.Tests.Helpers;
using GameCollection.Domain.Common;


namespace GameCollection.Tests.Services
{
    public class GameServiceTests
    {
        private readonly Mock<IGameRepository> _mockGameRepository;
        private readonly Mock<IRepository<Platform>> _mockPlatformRepository;
        private readonly Mock<IRepository<Genre>> _mockGenreRepository;
        private readonly Mock<IRepository<Franchise>> _mockFranchiseRepository;
        private readonly IMapper _mapper;
        private readonly GameService _gameService;

        public GameServiceTests()
        {
            _mockGameRepository = new Mock<IGameRepository>();
            _mockPlatformRepository = new Mock<IRepository<Platform>>();
            _mockGenreRepository = new Mock<IRepository<Genre>>();
            _mockFranchiseRepository = new Mock<IRepository<Franchise>>();

            // AutoMapper Settings
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = config.CreateMapper();

            _gameService = new GameService(
                _mockGameRepository.Object,
                _mockPlatformRepository.Object,
                _mockGenreRepository.Object,
                _mockFranchiseRepository.Object,
                _mapper);
        }


        [Fact]
        public async Task GetAllGamesAsync_ShouldReturnAllGames()
        {
            // Arrange
            var games = new List<Game>()
            {
                new Game { Id = 1, Title = "Game 1", ReleaseYear = 2020},
                new Game { Id = 2, Title = "Game 2", ReleaseYear = 2021}
            };

            _mockGameRepository.Setup(repo => repo.GetGamesWithDetailsAsync())
                .ReturnsAsync(games);

            // Act
            var result = await _gameService.GetAllGamesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }


        [Fact]
        public async Task GetGameByIdAsync_ExistingId_ShouldReturnGame()
        {
            // Arrange
            var game = new Game { Id = 1, Title = "Test Game", ReleaseYear = 2020 };
            _mockGameRepository.Setup(repo => repo.GetGameWithDetailsAsync(1))
                .ReturnsAsync(game);

            // Act
            var result = await _gameService.GetGameByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(game.Title, result.Title);
        }


        [Fact]
        public async Task GetGameByIdAsync_NonExistingId_ShouldReturnNull()
        {
            // Arrange
            _mockGameRepository.Setup(repo => repo.GetGameWithDetailsAsync(99999))
                .ReturnsAsync((Game?)null);

            // Act
            var result = await _gameService.GetGameByIdAsync(99999);

            // Assert
            Assert.Null(result);
        }


        [Fact]
        public async Task CreateGameAsync_ValidData_ShouldCreateGame()
        {
            // Arrange
            var gameDto = new CreateGameDto
            {
                Title = "New Game",
                ReleaseYear = 2023,
                PlatformIds = new List<int> { 1, 2},
                GenreIds = new List<int> { 3 }
            };

            var platform1 = new Platform { Id = 1, Name = "PC" };
            var platform2 = new Platform { Id = 2, Name = "PS5" };
            var genre = new Genre { Id = 3, Name = "Action" };

            _mockPlatformRepository.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(platform1);
            _mockPlatformRepository.Setup(repo => repo.GetByIdAsync(2))
                .ReturnsAsync(platform2);
            _mockGenreRepository.Setup(repo => repo.GetByIdAsync(3))
                .ReturnsAsync(genre);

            _mockGameRepository.Setup(repo => repo.AddAsync(It.IsAny<Game>()))
                .ReturnsAsync((Game game) =>
                {
                    game.Id = 1;
                    return game;
                });

            // Act
            var result = await _gameService.CreateGameAsync(gameDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("New Game", result.Title);
            Assert.Equal(2023, result.ReleaseYear);
        }


        [Fact]
        public async Task CreateGameAsync_InvalidPlatformId_ShouldThrowException()
        {
            // Arrange
            var gameDto = new CreateGameDto
            {
                Title = "New Game",
                ReleaseYear = 2023,
                PlatformIds = new List<int> { 999 } // Non-existing Platform
            };

            _mockPlatformRepository.Setup(repo => repo.GetByIdAsync(999))
                .ReturnsAsync((Platform?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => _gameService.CreateGameAsync(gameDto));

            Assert.Contains("Platform with ID 999 not found", exception.Message);
        }


        [Fact]
        public async Task CreateGameAsync_MissingTitle_ShouldThrowException()
        {
            // Arrange
            var gameDto = new CreateGameDto
            {
                Title = "",
                ReleaseYear = 2023
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => _gameService.CreateGameAsync(gameDto));

            Assert.Contains("Title is required", exception.Message);
        }


        [Fact]
        public async Task SearchGamesAsync_ValidQuery_ShouldReturnResults()
        {
            // Arrange
            var searchTerm = "zelda";
            var games = new List<Game>
            {
                new Game { Id = 1, Title = "The Legend of Zelda", ReleaseYear = 2017}
            };

            _mockGameRepository.Setup(repo => repo.SearchGamesAsync(searchTerm))
                .ReturnsAsync(games);

            // Act
            var result = await _gameService.SearchGamesAsync(searchTerm);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Contains("Zelda", result.First().Title);
        }


        [Fact]
        public async Task DeleteGameAsync_ExistingGame_ShouldReturnTrue()
        {
            // Arrange
            var game = new Game { Id = 1, Title = "Game to delete", ReleaseYear = 2020 };
            _mockGameRepository.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(game);

            // Act
            var result = await _gameService.DeleteGameAsync(1);

            // Assert
            Assert.True(result);
            _mockGameRepository.Verify(repo => repo.DeleteAsync(game), Times.Once);
        }


        [Fact]
        public async Task DeleteGameAsync_NonExistingGame_ShouldReturnFalse()
        {
            // Arrange
            _mockGameRepository.Setup(repo => repo.GetByIdAsync(99999))
                .ReturnsAsync((Game?)null);

            // Act
            var result = await _gameService.DeleteGameAsync(99999);

            // Assert
            Assert.False(result);
        }
    }
}
