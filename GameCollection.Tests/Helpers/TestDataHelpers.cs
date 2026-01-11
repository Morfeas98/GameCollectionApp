using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameCollection.Domain.Entities;

namespace GameCollection.Tests.Helpers
{
    public static class TestDataHelpers
    {
        public static List<Game> GetTestGames()
        {
            return new List<Game>()
            {
                new Game { Id = 1, Title = "The Witcher 3", ReleaseYear = 2015, MetacriticScore = 93},
                new Game { Id = 2, Title = "Red Dead Redemption 2", ReleaseYear = 2018, MetacriticScore = 97},
                new Game { Id = 3, Title = "Cyberpunk 2077", ReleaseYear = 2020, MetacriticScore = 86}
            };
        }

        public static List<Platform> GetTestPlatforms()
        {
            return new List<Platform>()
            {
                new Platform { Id = 1, Name = "PC"},
                new Platform { Id = 2, Name = "PlayStation 5"},
                new Platform { Id = 3, Name = "Xbox Series X"}
            };
        }

        public static List<Genre> GetTestGenres()
        {
            return new List<Genre>()
            {
                new Genre { Id = 1, Name = "RPG"},
                new Genre { Id = 2, Name = "Action"},
                new Genre { Id = 3, Name = "Adventure"}
            };
        }
    }
}
