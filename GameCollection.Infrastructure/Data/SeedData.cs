using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameCollection.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameCollection.Infrastructure.Data
{
    public static class SeedData 
    {
        public static void Initialize(AppDbContext context)
        {
            // Checks if DB is Created
            context.Database.EnsureCreated();

            // Returns if Already Populated
            if (context.Platforms.Any())
                return;

            // Add Platforms
            var platforms = new[]
            {
                new Platform { Name = "PC", Manufacturer = "Various"},
                new Platform { Name = "PlayStation 5", Manufacturer = "Sony"},
                new Platform { Name = "Xbox Series X", Manufacturer = "Microsoft"},
                new Platform { Name = "Nintendo Switch", Manufacturer = "Nintendo"}
            };

            context.Platforms.AddRange(platforms);

            // Add Genres
            var genres = new[]
            {
                new Genre { Name = "Action"},
                new Genre { Name = "Adventure"},
                new Genre { Name = "RPG"},
                new Genre { Name = "FPS"},
                new Genre { Name = "Strategy"}
            };

            context.Genres.AddRange(genres);

            // Add Franchises
            var franchises = new[]
            {
                new Franchise { Name = "The Legend of Zelda", Description = "Nintendo's iconic adventure series"},
                new Franchise { Name = "Final Fantasy", Description = "Square Enix's legendary RPG series"},
                new Franchise { Name = "Pokémon", Description = "The worldwide phenomenon"}
            };

            context.Franchises.AddRange(franchises);

            context.SaveChanges();

            // Add Games
            var zeldaFranchise = context.Franchises.First(f => f.Name == "The Legend of Zelda");

            var games = new[]
            {
                new Game
                {
                    Title = "The Legend of Zelda: Breath of the Wild",
                    ReleaseYear = 2017,
                    Developer = "Nintendo EPD",
                    Publisher = "Nintendo",
                    MetacriticScore = 97,
                    FranchiseId = zeldaFranchise.Id,
                },
                new Game
                {
                    Title = "Cyberpunk 2077",
                    ReleaseYear = 2020,
                    Developer = "CD Projekt Red",
                    Publisher = "CD Projekt",
                    MetacriticScore = 86
                }
            };

            context.Games.AddRange(games);
            context.SaveChanges();

            // Add GamePlatform
            var pcPlatform = context.Platforms.First(p => p.Name == "PC");
            var switchPlatform = context.Platforms.First(p => p.Name == "Nintendo Switch");
            var cyberpunk = context.Games.First(p => p.Title == "Cyberpunk 2077");
            var zelda = context.Games.First(p => p.Title.Contains("Zelda"));

            var gamePlatforms = new[]
            {
                new GamePlatform { GameId = cyberpunk.Id, PlatformId = pcPlatform.Id},
                new GamePlatform { GameId = cyberpunk.Id, PlatformId = context.Platforms.First(p => p.Name == "Playstation 5").Id},
                new GamePlatform { GameId = zelda.Id, PlatformId = switchPlatform.Id}
            };

            context.GamePlatforms.AddRange(gamePlatforms);
            context.SaveChanges();

            // Add Users
            if (!context.Users.Any())
            {
                var users = new[]
                {
                    new User
                    {
                        Username = "admin",
                        Email = "admin@gamecollection.com",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                        Role = "Admin"
                    },
                    new User
                    {
                        Username = "testuser",
                        Email = "user@gamecollection.com",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("User123!"),
                        Role = "User"
                    }
                };

                context.Users.AddRange(users);
                context.SaveChanges();
            }
        }
    }
}
