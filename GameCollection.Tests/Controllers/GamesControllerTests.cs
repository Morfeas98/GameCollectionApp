using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using GameCollection.API.Controllers;
using GameCollection.Application.Services;
using GameCollection.Application.DTOs;

namespace GameCollection.Tests.Controllers
{
    public class GamesControllerTests
    {
        private readonly Mock<IGameService> _mockGameService;
        private readonly GamesController _controller;

        public GamesControllerTests()
        {
            _mockGameService = new Mock<IGameService>();
            _controller = new GamesController(_mockGameService.Object);
        }


        [Fact]
        public async Task GetGames_ShouldReturnOkResultWithGames()
        {
            // Arrange
            var games = new List<GameDto>()
            {
                new GameDto { Id = 1, Title = "Game 1"},
                new GameDto { Id = 2, Title = "Game 2"}
            };

            _mockGameService.Setup(service => service.GetAllGamesAsync())
                .ReturnsAsync(games);

            // Act
            var result = await _controller.GetGames();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedGames = Assert.IsType<List<GameDto>>(okResult.Value);
            Assert.Equal(2, returnedGames.Count);
        }


        [Fact]
        public async Task GetGame_ExistingId_ShouldReturnGame()
        {
            // Arrange
            var game = new GameDto { Id = 1, Title = "Test Game" };
            _mockGameService.Setup(service => service.GetGameByIdAsync(1))
                .ReturnsAsync(game);

            // Act
            var result = await _controller.GetGame(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedGame = Assert.IsType<GameDto>(okResult.Value);
            Assert.Equal(game.Id, returnedGame.Id);
        }


        [Fact]
        public async Task GetGame_NonExistingId_ShouldReturnNotFound()
        {
            // Arrange
            _mockGameService.Setup(service => service.GetGameByIdAsync(99999))
                .ReturnsAsync((GameDto?)null);

            // Act
            var result = await _controller.GetGame(99999);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }


        [Fact]
        public async Task CreateGame_ValidData_ShouldReturnCreated()
        {
            // Arrange
            var createDto = new CreateGameDto { Title = "New Game", ReleaseYear = 2023 };
            var createdGame = new GameDto { Id = 1, Title = "New Game" };

            _mockGameService.Setup(service => service.CreateGameAsync(createDto))
                .ReturnsAsync(createdGame);

            // Act
            var result = await _controller.CreateGame(createDto);

            // Arrange
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(nameof(_controller.GetGame), createdResult.ActionName);
            Assert.Equal(1, createdGame.Id);
        }


        [Fact]
        public async Task SearchGames_ValidQuery_ShouldReturnResults()
        {
            // Arrange
            var query = "zelda";
            var games = new List<GameDto>
            {
                new GameDto { Id = 1, Title = "The Legend of Zelda"}
            };

            _mockGameService.Setup(service => service.SearchGamesAsync(query))
                .ReturnsAsync(games);

            // Act
            var result = await _controller.SearchGames(query);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedGames = Assert.IsType<List<GameDto>>(okResult.Value);
            Assert.Single(returnedGames);
        }
    }
}
