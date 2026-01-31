using System;
using System.Collections.Generic;
using System.Linq;
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

            // Add Platforms (Εμπλουτισμός με 15+ πλατφόρμες)
            var platforms = new[]
            {
                new Platform { Name = "PC", Manufacturer = "Various", ReleaseYear = "1970" },
                new Platform { Name = "PlayStation 5", Manufacturer = "Sony", ReleaseYear = "2020" },
                new Platform { Name = "PlayStation 4", Manufacturer = "Sony", ReleaseYear = "2013" },
                new Platform { Name = "PlayStation 3", Manufacturer = "Sony", ReleaseYear = "2006" },
                new Platform { Name = "Xbox Series X", Manufacturer = "Microsoft", ReleaseYear = "2020" },
                new Platform { Name = "Xbox One", Manufacturer = "Microsoft", ReleaseYear = "2013" },
                new Platform { Name = "Xbox 360", Manufacturer = "Microsoft", ReleaseYear = "2005" },
                new Platform { Name = "Nintendo Switch", Manufacturer = "Nintendo", ReleaseYear = "2017" },
                new Platform { Name = "Nintendo Wii U", Manufacturer = "Nintendo", ReleaseYear = "2012" },
                new Platform { Name = "Nintendo 3DS", Manufacturer = "Nintendo", ReleaseYear = "2011" },
                new Platform { Name = "Nintendo DS", Manufacturer = "Nintendo", ReleaseYear = "2004" },
                new Platform { Name = "PlayStation Vita", Manufacturer = "Sony", ReleaseYear = "2011" },
                new Platform { Name = "PlayStation Portable", Manufacturer = "Sony", ReleaseYear = "2004" },
                new Platform { Name = "iOS", Manufacturer = "Apple", ReleaseYear = "2007" },
                new Platform { Name = "Android", Manufacturer = "Google", ReleaseYear = "2008" },
                new Platform { Name = "Stadia", Manufacturer = "Google", ReleaseYear = "2019" },
                new Platform { Name = "PlayStation 2", Manufacturer = "Sony", ReleaseYear = "2000" }
            };

            context.Platforms.AddRange(platforms);

            // Add Genres (Εμπλουτισμός με 20+ είδη)
            var genres = new[]
            {
                new Genre { Name = "Action" },
                new Genre { Name = "Adventure" },
                new Genre { Name = "RPG" },
                new Genre { Name = "FPS" },
                new Genre { Name = "Strategy" },
                new Genre { Name = "Simulation" },
                new Genre { Name = "Sports" },
                new Genre { Name = "Racing" },
                new Genre { Name = "Fighting" },
                new Genre { Name = "Horror" },
                new Genre { Name = "Survival" },
                new Genre { Name = "Stealth" },
                new Genre { Name = "MMO" },
                new Genre { Name = "MOBA" },
                new Genre { Name = "Battle Royale" },
                new Genre { Name = "Puzzle" },
                new Genre { Name = "Platformer" },
                new Genre { Name = "Music/Rhythm" },
                new Genre { Name = "Educational" },
                new Genre { Name = "Visual Novel" },
                new Genre { Name = "Open World" },
                new Genre { Name = "Sandbox" }
            };

            context.Genres.AddRange(genres);

            // Add Franchises (Εμπλουτισμός με 25+ franchise)
            var franchises = new[]
            {
                new Franchise { Name = "The Legend of Zelda", Description = "Nintendo's iconic adventure series" },
                new Franchise { Name = "Final Fantasy", Description = "Square Enix's legendary RPG series" },
                new Franchise { Name = "Pokémon", Description = "The worldwide phenomenon" },
                new Franchise { Name = "Super Mario", Description = "Nintendo's flagship platforming series" },
                new Franchise { Name = "The Elder Scrolls", Description = "Bethesda's epic fantasy RPG series" },
                new Franchise { Name = "Fallout", Description = "Post-apocalyptic RPG series" },
                new Franchise { Name = "Grand Theft Auto", Description = "Rockstar's open-world crime series" },
                new Franchise { Name = "Call of Duty", Description = "First-person shooter franchise" },
                new Franchise { Name = "Battlefield", Description = "Large-scale military shooter series" },
                new Franchise { Name = "Assassin's Creed", Description = "Historical action-adventure series" },
                new Franchise { Name = "Resident Evil", Description = "Survival horror series" },
                new Franchise { Name = "Metal Gear", Description = "Stealth action series" },
                new Franchise { Name = "The Witcher", Description = "Fantasy RPG based on Andrzej Sapkowski's books" },
                new Franchise { Name = "Dark Souls", Description = "Challenging action RPG series" },
                new Franchise { Name = "Halo", Description = "Microsoft's flagship sci-fi shooter" },
                new Franchise { Name = "God of War", Description = "Action-adventure mythology series" },
                new Franchise { Name = "Uncharted", Description = "Action-adventure treasure hunting series" },
                new Franchise { Name = "The Last of Us", Description = "Post-apocalyptic narrative-driven series" },
                new Franchise { Name = "Mass Effect", Description = "Sci-fi RPG series" },
                new Franchise { Name = "Dragon Age", Description = "Fantasy RPG series" },
                new Franchise { Name = "Borderlands", Description = "Looter-shooter RPG series" },
                new Franchise { Name = "BioShock", Description = "Narrative-driven FPS series" },
                new Franchise { Name = "Diablo", Description = "Action RPG dungeon crawler" },
                new Franchise { Name = "Street Fighter", Description = "Classic fighting game series" },
                new Franchise { Name = "Mortal Kombat", Description = "Brutal fighting game series" },
                new Franchise { Name = "Forza", Description = "Racing simulation series" },
                new Franchise { Name = "FIFA", Description = "Football simulation series" }
            };

            context.Franchises.AddRange(franchises);
            context.SaveChanges();

            // Add Games (Εμπλουτισμός με 50+ παιχνίδια)
            var zeldaFranchise = context.Franchises.First(f => f.Name == "The Legend of Zelda");
            var marioFranchise = context.Franchises.First(f => f.Name == "Super Mario");
            var finalFantasyFranchise = context.Franchises.First(f => f.Name == "Final Fantasy");
            var elderScrollsFranchise = context.Franchises.First(f => f.Name == "The Elder Scrolls");
            var falloutFranchise = context.Franchises.First(f => f.Name == "Fallout");
            var gtaFranchise = context.Franchises.First(f => f.Name == "Grand Theft Auto");
            var witcherFranchise = context.Franchises.First(f => f.Name == "The Witcher");
            var assassinFranchise = context.Franchises.First(f => f.Name == "Assassin's Creed");
            var residentEvilFranchise = context.Franchises.First(f => f.Name == "Resident Evil");
            var darkSoulsFranchise = context.Franchises.First(f => f.Name == "Dark Souls");

            var games = new[]
            {
                new Game
                {
                    Title = "The Legend of Zelda: Breath of the Wild",
                    ReleaseYear = 2017,
                    Developer = "Nintendo EPD",
                    Publisher = "Nintendo",
                    ImageUrl = "https://static0.cbrimages.com/wordpress/wp-content/uploads/sharedimages/2025/02/zelda-breath-of-the-wild-cove.jpg",
                    MetacriticScore = 97,
                    MetacriticUrl = "https://www.metacritic.com/game/the-legend-of-zelda-breath-of-the-wild/",
                    FranchiseId = zeldaFranchise.Id,
                    Description = "Open-world adventure in the kingdom of Hyrule"
                },
                new Game
                {
                    Title = "Cyberpunk 2077",
                    ReleaseYear = 2020,
                    Developer = "CD Projekt Red",
                    Publisher = "CD Projekt",
                    ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRWH8K2baa5QsbGLwLgdOBoCgAH8G-tts6ljcfY3mdpYjy4QjtJxuaAPeTM3asitg9ti_Mf&s=10",
                    MetacriticScore = 86,
                    MetacriticUrl = "https://www.metacritic.com/game/cyberpunk-2077/",
                    Description = "Open-world RPG set in Night City"
                },
                new Game
                {
                    Title = "The Legend of Zelda: Tears of the Kingdom",
                    ReleaseYear = 2023,
                    Developer = "Nintendo EPD",
                    Publisher = "Nintendo",
                    ImageUrl = "https://kids.kiddle.co/images/f/fb/The_Legend_of_Zelda_Tears_of_the_Kingdom_cover.jpg",
                    MetacriticScore = 96,
                    MetacriticUrl = "https://www.metacritic.com/game/the-legend-of-zelda-tears-of-the-kingdom/",
                    FranchiseId = zeldaFranchise.Id,
                    Description = "Sequel to Breath of the Wild"
                },
                new Game
                {
                    Title = "Super Mario Odyssey",
                    ReleaseYear = 2017,
                    Developer = "Nintendo EPD",
                    Publisher = "Nintendo",
                    ImageUrl = "https://images.igdb.com/igdb/image/upload/t_cover_big/co1mxf.webp",
                    MetacriticScore = 97,
                    MetacriticUrl = "https://www.metacritic.com/game/super-mario-odyssey/",
                    FranchiseId = marioFranchise.Id,
                    Description = "3D platformer with capture mechanic"
                },
                new Game
                {
                    Title = "Final Fantasy VII Remake",
                    ReleaseYear = 2020,
                    Developer = "Square Enix",
                    Publisher = "Square Enix",
                    ImageUrl = "https://images.igdb.com/igdb/image/upload/t_cover_big/cobcwt.webp",
                    MetacriticScore = 87,
                    MetacriticUrl = "https://www.metacritic.com/game/final-fantasy-vii-remake/",
                    FranchiseId = finalFantasyFranchise.Id,
                    Description = "Reimagining of the classic RPG"
                },
                new Game
                {
                    Title = "The Elder Scrolls V: Skyrim",
                    ReleaseYear = 2011,
                    Developer = "Bethesda Game Studios",
                    Publisher = "Bethesda Softworks",
                    ImageUrl = "https://images.igdb.com/igdb/image/upload/t_cover_big/co1tnw.webp",
                    MetacriticScore = 94,
                    MetacriticUrl = "https://www.metacritic.com/game/the-elder-scrolls-v-skyrim/",
                    FranchiseId = elderScrollsFranchise.Id,
                    Description = "Open-world fantasy RPG"
                },
                new Game
                {
                    Title = "Fallout 4",
                    ReleaseYear = 2015,
                    Developer = "Bethesda Game Studios",
                    Publisher = "Bethesda Softworks",
                    ImageUrl = "https://images.igdb.com/igdb/image/upload/t_cover_big/co1yc6.webp",
                    MetacriticScore = 84,
                    MetacriticUrl = "https://www.metacritic.com/game/fallout-4/",
                    FranchiseId = falloutFranchise.Id,
                    Description = "Post-apocalyptic RPG"
                },
                new Game
                {
                    Title = "Grand Theft Auto V",
                    ReleaseYear = 2013,
                    Developer = "Rockstar North",
                    Publisher = "Rockstar Games",
                    ImageUrl = "https://images.igdb.com/igdb/image/upload/t_cover_big/co2lbd.webp",
                    MetacriticScore = 97,
                    MetacriticUrl = "https://www.metacritic.com/game/grand-theft-auto-v/",
                    FranchiseId = gtaFranchise.Id,
                    Description = "Open-world crime game"
                },
                new Game
                {
                    Title = "Red Dead Redemption 2",
                    ReleaseYear = 2018,
                    Developer = "Rockstar Studios",
                    Publisher = "Rockstar Games",
                    ImageUrl = "https://images.igdb.com/igdb/image/upload/t_cover_big/co1q1f.webp",
                    MetacriticScore = 97,
                    MetacriticUrl = "https://www.metacritic.com/game/red-dead-redemption-2/",
                    Description = "Western-themed action-adventure"
                },
                new Game
                {
                    Title = "The Witcher 3: Wild Hunt",
                    ReleaseYear = 2015,
                    Developer = "CD Projekt Red",
                    Publisher = "CD Projekt",
                    ImageUrl = "https://images.igdb.com/igdb/image/upload/t_cover_big/coaarl.webp",
                    MetacriticScore = 93,
                    MetacriticUrl = "https://www.metacritic.com/game/the-witcher-3-wild-hunt/",
                    FranchiseId = witcherFranchise.Id,
                    Description = "Open-world fantasy RPG"
                },
                new Game
                {
                    Title = "Dark Souls III",
                    ReleaseYear = 2016,
                    Developer = "FromSoftware",
                    Publisher = "Bandai Namco",
                    ImageUrl = "https://images.igdb.com/igdb/image/upload/t_cover_big/cob9ed.webp",
                    MetacriticScore = 89,
                    MetacriticUrl = "https://www.metacritic.com/game/dark-souls-iii/",
                    FranchiseId = darkSoulsFranchise.Id,
                    Description = "Challenging action RPG"
                },
                new Game
                {
                    Title = "Elden Ring",
                    ReleaseYear = 2022,
                    Developer = "FromSoftware",
                    Publisher = "Bandai Namco",
                    ImageUrl = "https://images.igdb.com/igdb/image/upload/t_cover_big/co4jni.webp",
                    MetacriticScore = 96,
                    MetacriticUrl = "https://www.metacritic.com/game/elden-ring/",
                    Description = "Open-world action RPG"
                },
                new Game
                {
                    Title = "God of War (2018)",
                    ReleaseYear = 2018,
                    Developer = "Santa Monica Studio",
                    Publisher = "Sony Interactive Entertainment",
                    ImageUrl = "https://images.igdb.com/igdb/image/upload/t_cover_big/cob9h2.webp",
                    MetacriticScore = 94,
                    MetacriticUrl = "https://www.metacritic.com/game/god-of-war/",
                    Description = "Action-adventure set in Norse mythology"
                },
                new Game
                {
                    Title = "Horizon Zero Dawn",
                    ReleaseYear = 2017,
                    Developer = "Guerrilla Games",
                    Publisher = "Sony Interactive Entertainment",
                    ImageUrl = "https://images.igdb.com/igdb/image/upload/t_cover_big/co2una.webp",
                    MetacriticScore = 89,
                    MetacriticUrl = "https://www.metacritic.com/game/horizon-zero-dawn/",
                    Description = "Open-world action RPG with robotic creatures"
                },
                new Game
                {
                    Title = "Persona 5 Royal",
                    ReleaseYear = 2020,
                    Developer = "Atlus",
                    Publisher = "Sega",
                    ImageUrl = "https://images.igdb.com/igdb/image/upload/t_cover_big/cobaqh.webp",
                    MetacriticScore = 95,
                    MetacriticUrl = "https://www.metacritic.com/game/persona-5-royal/",
                    Description = "Japanese RPG with social simulation"
                },
                new Game
                {
                    Title = "Hades",
                    ReleaseYear = 2020,
                    Developer = "Supergiant Games",
                    Publisher = "Supergiant Games",
                    ImageUrl = "https://images.igdb.com/igdb/image/upload/t_cover_big/cob9kr.webp",
                    MetacriticScore = 93,
                    MetacriticUrl = "https://www.metacritic.com/game/hades/",
                    Description = "Roguelike dungeon crawler"
                },
                new Game
                {
                    Title = "Stardew Valley",
                    ReleaseYear = 2016,
                    Developer = "ConcernedApe",
                    Publisher = "Chucklefish",
                    ImageUrl = "https://images.igdb.com/igdb/image/upload/t_cover_big/coa93h.webp",
                    MetacriticScore = 89,
                    MetacriticUrl = "https://www.metacritic.com/game/stardew-valley/",
                    Description = "Farming simulation RPG"
                },
                new Game
                {
                    Title = "Minecraft",
                    ReleaseYear = 2011,
                    Developer = "Mojang Studios",
                    Publisher = "Mojang Studios",
                    ImageUrl = "https://images.igdb.com/igdb/image/upload/t_cover_big/co8fu7.webp",
                    MetacriticScore = 93,
                    MetacriticUrl = "https://www.metacritic.com/game/minecraft/",
                    Description = "Sandbox building game"
                },
                new Game
                {
                    Title = "Portal 2",
                    ReleaseYear = 2011,
                    Developer = "Valve",
                    Publisher = "Valve",
                    ImageUrl = "https://images.igdb.com/igdb/image/upload/t_cover_big/co1rs4.webp",
                    MetacriticScore = 95,
                    MetacriticUrl = "https://www.metacritic.com/game/portal-2/",
                    Description = "Puzzle-platformer"
                },
                new Game
                {
                    Title = "Half-Life: Alyx",
                    ReleaseYear = 2020,
                    Developer = "Valve",
                    Publisher = "Valve",
                    ImageUrl = "https://images.igdb.com/igdb/image/upload/t_cover_big/co87vg.webp",
                    MetacriticScore = 93,
                    MetacriticUrl = "https://www.metacritic.com/game/half-life-alyx/",
                    Description = "VR first-person shooter"
                },
                new Game
                {
                    Title = "Resident Evil 2 Remake",
                    ReleaseYear = 2019,
                    Developer = "Capcom",
                    Publisher = "Capcom",
                    ImageUrl = "https://images.igdb.com/igdb/image/upload/t_cover_big/co1ir3.webp",
                    MetacriticScore = 91,
                    MetacriticUrl = "https://www.metacritic.com/game/resident-evil-2/",
                    FranchiseId = residentEvilFranchise.Id,
                    Description = "Survival horror remake"
                },
                new Game
                {
                    Title = "Resident Evil Village",
                    ReleaseYear = 2021,
                    Developer = "Capcom",
                    Publisher = "Capcom",
                    ImageUrl = "https://images.igdb.com/igdb/image/upload/t_cover_big/coab9q.webp",
                    MetacriticScore = 84,
                    MetacriticUrl = "https://www.metacritic.com/game/resident-evil-village/",
                    FranchiseId = residentEvilFranchise.Id,
                    Description = "Survival horror"
                },
                new Game
                {
                    Title = "Assassin's Creed Valhalla",
                    ReleaseYear = 2020,
                    Developer = "Ubisoft Montreal",
                    Publisher = "Ubisoft",
                    ImageUrl = "https://images.igdb.com/igdb/image/upload/t_cover_big/co2ed3.webp",
                    MetacriticScore = 82,
                    MetacriticUrl = "https://www.metacritic.com/game/assassins-creed-valhalla/",
                    FranchiseId = assassinFranchise.Id,
                    Description = "Action RPG set in Viking era"
                },
                new Game
                {
                    Title = "Doom Eternal",
                    ReleaseYear = 2020,
                    Developer = "id Software",
                    Publisher = "Bethesda Softworks",
                    ImageUrl = "https://images.igdb.com/igdb/image/upload/t_cover_big/co3p5n.webp",
                    MetacriticScore = 88,
                    MetacriticUrl = "https://www.metacritic.com/game/doom-eternal/",
                    Description = "Fast-paced first-person shooter"
                },
                new Game
                {
                    Title = "Animal Crossing: New Horizons",
                    ReleaseYear = 2020,
                    Developer = "Nintendo EPD",
                    Publisher = "Nintendo",
                    ImageUrl = "https://images.igdb.com/igdb/image/upload/t_cover_big/co3wls.webp",
                    MetacriticScore = 90,
                    MetacriticUrl = "https://www.metacritic.com/game/animal-crossing-new-horizons/",
                    Description = "Life simulation game"
                },
                new Game
                {
                    Title = "Celeste",
                    ReleaseYear = 2018,
                    Developer = "Maddy Makes Games",
                    Publisher = "Maddy Makes Games",
                    ImageUrl = "https://images.igdb.com/igdb/image/upload/t_cover_big/cob9dh.webp",
                    MetacriticScore = 92,
                    MetacriticUrl = "https://www.metacritic.com/game/celeste/",
                    Description = "Challenging platformer"
                },
                new Game
                {
                    Title = "Hollow Knight",
                    ReleaseYear = 2017,
                    Developer = "Team Cherry",
                    Publisher = "Team Cherry",
                    ImageUrl = "https://images.igdb.com/igdb/image/upload/t_cover_big/cob9sw.webp",
                    MetacriticScore = 90,
                    MetacriticUrl = "https://www.metacritic.com/game/hollow-knight/",
                    Description = "Metroidvania action-adventure"
                },
                new Game
                {
                    Title = "Disco Elysium",
                    ReleaseYear = 2019,
                    Developer = "ZA/UM",
                    Publisher = "ZA/UM",
                    ImageUrl = "https://images.igdb.com/igdb/image/upload/t_cover_big/co9j3v.webp",
                    MetacriticScore = 91,
                    MetacriticUrl = "https://www.metacritic.com/game/disco-elysium/",
                    Description = "Detective RPG"
                },
                new Game
                {
                    Title = "Death Stranding",
                    ReleaseYear = 2019,
                    Developer = "Kojima Productions",
                    Publisher = "Sony Interactive Entertainment",
                    ImageUrl = "https://images.igdb.com/igdb/image/upload/t_cover_big/co5vq8.webp",
                    MetacriticScore = 82,
                    MetacriticUrl = "https://www.metacritic.com/game/death-stranding/",
                    Description = "Action game set in post-apocalyptic America"
                },
                new Game
                {
                    Title = "Sekiro: Shadows Die Twice",
                    ReleaseYear = 2019,
                    Developer = "FromSoftware",
                    Publisher = "Activision",
                    ImageUrl = "https://images.igdb.com/igdb/image/upload/t_cover_big/co2a23.webp",
                    MetacriticScore = 90,
                    MetacriticUrl = "https://www.metacritic.com/game/sekiro-shadows-die-twice/",
                    Description = "Action-adventure set in feudal Japan"
                },
                new Game
                {
                    Title = "Control",
                    ReleaseYear = 2019,
                    Developer = "Remedy Entertainment",
                    Publisher = "505 Games",
                    ImageUrl = "https://images.igdb.com/igdb/image/upload/t_cover_big/co2evj.webp",
                    MetacriticScore = 85,
                    MetacriticUrl = "https://www.metacritic.com/game/control/",
                    Description = "Supernatural action-adventure"
                },
                new Game
                {
                    Title = "Returnal",
                    ReleaseYear = 2021,
                    Developer = "Housemarque",
                    Publisher = "Sony Interactive Entertainment",
                    ImageUrl = "https://images.igdb.com/igdb/image/upload/t_cover_big/co3wc1.webp",
                    MetacriticScore = 86,
                    MetacriticUrl = "https://www.metacritic.com/game/returnal/",
                    Description = "Roguelike third-person shooter"
                },
                new Game
                {
                    Title = "Ratchet & Clank: Rift Apart",
                    ReleaseYear = 2021,
                    Developer = "Insomniac Games",
                    Publisher = "Sony Interactive Entertainment",
                    ImageUrl = "https://images.igdb.com/igdb/image/upload/t_cover_big/co2str.webp",
                    MetacriticScore = 88,
                    MetacriticUrl = "https://www.metacritic.com/game/ratchet-and-clank-rift-apart/",
                    Description = "Action-platformer"
                },
                new Game
                {
                    Title = "It Takes Two",
                    ReleaseYear = 2021,
                    Developer = "Hazelight Studios",
                    Publisher = "Electronic Arts",
                    ImageUrl = "https://images.igdb.com/igdb/image/upload/t_cover_big/cob22v.webp",
                    MetacriticScore = 89,
                    MetacriticUrl = "https://www.metacritic.com/game/it-takes-two/",
                    Description = "Cooperative action-adventure"
                },
                new Game
                {
                    Title = "Kena: Bridge of Spirits",
                    ReleaseYear = 2021,
                    Developer = "Ember Lab",
                    Publisher = "Ember Lab",
                    ImageUrl = "https://images.igdb.com/igdb/image/upload/t_cover_big/co9hwy.webp",
                    MetacriticScore = 81,
                    MetacriticUrl = "https://www.metacritic.com/game/kena-bridge-of-spirits/",
                    Description = "Action-adventure with puzzle elements"
                },
                new Game
                {
                    Title = "Psychonauts 2",
                    ReleaseYear = 2021,
                    Developer = "Double Fine",
                    Publisher = "Xbox Game Studios",
                    ImageUrl = "https://images.igdb.com/igdb/image/upload/t_cover_big/co1sod.webp",
                    MetacriticScore = 89,
                    MetacriticUrl = "https://www.metacritic.com/game/psychonauts-2/",
                    Description = "Platformer set inside minds"
                },
                new Game
                {
                    Title = "Forza Horizon 5",
                    ReleaseYear = 2021,
                    Developer = "Playground Games",
                    Publisher = "Xbox Game Studios",
                    ImageUrl = "https://images.igdb.com/igdb/image/upload/t_cover_big/co3ofx.webp",
                    MetacriticScore = 92,
                    MetacriticUrl = "https://www.metacritic.com/game/forza-horizon-5/",
                    Description = "Open-world racing game"
                },
                new Game
                {
                    Title = "Metroid Dread",
                    ReleaseYear = 2021,
                    Developer = "MercurySteam",
                    Publisher = "Nintendo",
                    ImageUrl = "https://images.igdb.com/igdb/image/upload/t_cover_big/coba9g.webp",
                    MetacriticScore = 88,
                    MetacriticUrl = "https://www.metacritic.com/game/metroid-dread/",
                    Description = "Action-adventure platformer"
                },
                new Game
                {
                    Title = "Shin Megami Tensei V",
                    ReleaseYear = 2021,
                    Developer = "Atlus",
                    Publisher = "Sega",
                    ImageUrl = "https://images.igdb.com/igdb/image/upload/t_cover_big/co39zk.webp",
                    MetacriticScore = 84,
                    MetacriticUrl = "https://www.metacritic.com/search/shin%20megami%20tensei%20v/",
                    Description = "Japanese RPG"
                },
                new Game
                {
                    Title = "Back 4 Blood",
                    ReleaseYear = 2021,
                    Developer = "Turtle Rock Studios",
                    Publisher = "Warner Bros. Interactive",
                    ImageUrl = "https://images.igdb.com/igdb/image/upload/t_cover_big/co2mhj.webp",
                    MetacriticScore = 75,
                    MetacriticUrl = "https://www.metacritic.com/game/back-4-blood/",
                    Description = "Cooperative first-person shooter"
                },
                new Game
                {
                    Title = "Far Cry 6",
                    ReleaseYear = 2021,
                    Developer = "Ubisoft Toronto",
                    Publisher = "Ubisoft",
                    ImageUrl = "https://images.igdb.com/igdb/image/upload/t_cover_big/co2npg.webp",
                    MetacriticScore = 73,
                    MetacriticUrl = "https://www.metacritic.com/game/far-cry-6/",
                    Description = "First-person shooter"
                },
                new Game
                {
                    Title = "Battlefield 2042",
                    ReleaseYear = 2021,
                    Developer = "DICE",
                    Publisher = "Electronic Arts",
                    ImageUrl = "https://images.igdb.com/igdb/image/upload/t_cover_big/co7dfn.webp",
                    MetacriticScore = 68,
                    MetacriticUrl = "https://www.metacritic.com/game/battlefield-2042/",
                    Description = "First-person shooter"
                },
                new Game
                {
                    Title = "Call of Duty: Vanguard",
                    ReleaseYear = 2021,
                    Developer = "Sledgehammer Games",
                    Publisher = "Activision",
                    ImageUrl = "https://images.igdb.com/igdb/image/upload/t_cover_big/co3kq8.webp",
                    MetacriticScore = 73,
                    MetacriticUrl = "https://www.metacritic.com/game/call-of-duty-vanguard/",
                    Description = "First-person shooter"
                },
                new Game
                {
                    Title = "Age of Empires IV",
                    ReleaseYear = 2021,
                    Developer = "Relic Entertainment",
                    Publisher = "Xbox Game Studios",
                    ImageUrl = "https://images.igdb.com/igdb/image/upload/t_cover_big/co39tg.webp",
                    MetacriticScore = 81,
                    MetacriticUrl = "https://www.metacritic.com/game/age-of-empires-iv/",
                    Description = "Real-time strategy"
                },
                new Game
                {
                    Title = "Microsoft Flight Simulator (2020)",
                    ReleaseYear = 2020,
                    Developer = "Asobo Studio",
                    Publisher = "Xbox Game Studios",
                    ImageUrl = "https://images.igdb.com/igdb/image/upload/t_cover_big/co2dqk.webp",
                    MetacriticScore = 90,
                    MetacriticUrl = "https://www.metacritic.com/game/microsoft-flight-simulator/",
                    Description = "Flight simulation"
                },
                new Game
                {
                    Title = "Ghost of Tsushima",
                    ReleaseYear = 2020,
                    Developer = "Sucker Punch Productions",
                    Publisher = "Sony Interactive Entertainment",
                    ImageUrl = "https://images.igdb.com/igdb/image/upload/t_cover_big/co2crj.webp",
                    MetacriticScore = 83,
                    MetacriticUrl = "https://www.metacritic.com/game/ghost-of-tsushima/",
                    Description = "Action-adventure set in feudal Japan"
                },
                new Game
                {
                    Title = "The Last of Us Part II",
                    ReleaseYear = 2020,
                    Developer = "Naughty Dog",
                    Publisher = "Sony Interactive Entertainment",
                    ImageUrl = "https://images.igdb.com/igdb/image/upload/t_cover_big/co5ziw.webp",
                    MetacriticScore = 93,
                    MetacriticUrl = "https://www.metacritic.com/game/the-last-of-us-part-ii/",
                    Description = "Action-adventure survival horror"
                },
                new Game
                {
                    Title = "Spider-Man: Miles Morales",
                    ReleaseYear = 2020,
                    Developer = "Insomniac Games",
                    Publisher = "Sony Interactive Entertainment",
                    ImageUrl = "https://images.igdb.com/igdb/image/upload/t_cover_big/cob9yq.webp",
                    MetacriticScore = 85,
                    MetacriticUrl = "https://www.metacritic.com/game/marvels-spider-man-miles-morales/",
                    Description = "Action-adventure"
                },
                new Game
                {
                    Title = "Demon's Souls (2020)",
                    ReleaseYear = 2020,
                    Developer = "Bluepoint Games",
                    Publisher = "Sony Interactive Entertainment",
                    ImageUrl = "https://images.igdb.com/igdb/image/upload/t_cover_big/co2kj9.webp",
                    MetacriticScore = 92,
                    MetacriticUrl = "https://www.metacritic.com/game/demons-souls/",
                    Description = "Action RPG remake"
                },
                new Game
                {
                    Title = "Yakuza: Like a Dragon",
                    ReleaseYear = 2020,
                    Developer = "Ryū Ga Gotoku Studio",
                    Publisher = "Sega",
                    ImageUrl = "https://images.igdb.com/igdb/image/upload/t_cover_big/co2em9.webp",
                    MetacriticScore = 84,
                    MetacriticUrl = "https://www.metacritic.com/game/yakuza-like-a-dragon/",
                    Description = "Role-playing game"
                },
                new Game
                {
                    Title = "Baldur's Gate 3",
                    ReleaseYear = 2023,
                    Developer = "Larian Studios",
                    Publisher = "Larian Studios",
                    ImageUrl = "https://images.igdb.com/igdb/image/upload/t_cover_big/co670h.webp",
                    MetacriticScore = 96,
                    MetacriticUrl = "https://www.metacritic.com/game/baldurs-gate-3/",
                    Description = "Dungeons & Dragons RPG"
                },
                new Game
                {
                    Title = "Resident Evil Requiem",
                    ReleaseYear = 2026,
                    Developer = "Capcom",
                    Publisher = "Capcom",
                    ImageUrl = "https://images.igdb.com/igdb/image/upload/t_cover_big/cob3bo.webp",
                    MetacriticScore = 95,
                    MetacriticUrl = "https://www.metacritic.com/game/resident-evil-requiem/",
                    FranchiseId = residentEvilFranchise.Id,
                    Description = "Resident Evil Requiem is the ninth entry in the Resident Evil series"
                },
                new Game
                {
                    Title = "Nioh 3",
                    ReleaseYear = 2026,
                    Developer = "Team NINJA",
                    Publisher = "Koei Temco Games",
                    ImageUrl = "https://images.igdb.com/igdb/image/upload/t_cover_big/co9x1e.webp",
                    MetacriticScore = 99,
                    MetacriticUrl = "https://www.metacritic.com/game/nioh-3/",
                    Description = "In the latest game in the dark samurai action RPG series \"Nioh,\" you will need to use both Samurai and Ninja combat styles in your battles against formidable yokai as you explore a thrilling open world"
                }
            };

            context.Games.AddRange(games);
            context.SaveChanges();

            // Add GamePlatform (Συσχέτιση παιχνιδιών με πολλαπλές πλατφόρμες)
            var pcPlatform = context.Platforms.First(p => p.Name == "PC");
            var ps5Platform = context.Platforms.First(p => p.Name == "PlayStation 5");
            var ps4Platform = context.Platforms.First(p => p.Name == "PlayStation 4");
            var xboxSeriesPlatform = context.Platforms.First(p => p.Name == "Xbox Series X");
            var xboxOnePlatform = context.Platforms.First(p => p.Name == "Xbox One");
            var switchPlatform = context.Platforms.First(p => p.Name == "Nintendo Switch");
            var xbox360Platform = context.Platforms.First(p => p.Name == "Xbox 360");
            var ps3Platform = context.Platforms.First(p => p.Name == "PlayStation 3");
            var iosPlatform = context.Platforms.First(p => p.Name == "iOS");
            var androidPlatform = context.Platforms.First(p => p.Name == "Android");
            var stadiaPlatform = context.Platforms.First(p => p.Name == "Stadia");
            var ps2Platform = context.Platforms.First(p => p.Name == "PlayStation 2");

            // Λήψη παιχνιδιών για συσχέτιση
            var cyberpunk = context.Games.First(g => g.Title == "Cyberpunk 2077");
            var zeldaBotw = context.Games.First(g => g.Title == "The Legend of Zelda: Breath of the Wild");
            var zeldaTotk = context.Games.First(g => g.Title == "The Legend of Zelda: Tears of the Kingdom");
            var marioOdyssey = context.Games.First(g => g.Title == "Super Mario Odyssey");
            var ff7Remake = context.Games.First(g => g.Title == "Final Fantasy VII Remake");
            var skyrim = context.Games.First(g => g.Title == "The Elder Scrolls V: Skyrim");
            var fallout4 = context.Games.First(g => g.Title == "Fallout 4");
            var gta5 = context.Games.First(g => g.Title == "Grand Theft Auto V");
            var rdr2 = context.Games.First(g => g.Title == "Red Dead Redemption 2");
            var witcher3 = context.Games.First(g => g.Title == "The Witcher 3: Wild Hunt");
            var darkSouls3 = context.Games.First(g => g.Title == "Dark Souls III");
            var eldenRing = context.Games.First(g => g.Title == "Elden Ring");
            var godOfWar = context.Games.First(g => g.Title == "God of War (2018)");
            var horizon = context.Games.First(g => g.Title == "Horizon Zero Dawn");
            var persona5 = context.Games.First(g => g.Title == "Persona 5 Royal");
            var hades = context.Games.First(g => g.Title == "Hades");
            var stardew = context.Games.First(g => g.Title == "Stardew Valley");
            var minecraft = context.Games.First(g => g.Title == "Minecraft");
            var portal2 = context.Games.First(g => g.Title == "Portal 2");
            var re2 = context.Games.First(g => g.Title == "Resident Evil 2 Remake");
            var acValhalla = context.Games.First(g => g.Title == "Assassin's Creed Valhalla");
            var doomEternal = context.Games.First(g => g.Title == "Doom Eternal");
            var animalCrossing = context.Games.First(g => g.Title == "Animal Crossing: New Horizons");
            var lastOfUs2 = context.Games.First(g => g.Title == "The Last of Us Part II");
            var ghostOfTsushima = context.Games.First(g => g.Title == "Ghost of Tsushima");
            var spiderman = context.Games.First(g => g.Title == "Spider-Man: Miles Morales");
            var demonsSouls = context.Games.First(g => g.Title == "Demon's Souls (2020)");
            var reRequiem = context.Games.First(g => g.Title == "Resident Evil Requiem");

            var gamePlatforms = new List<GamePlatform>
            {
                // Cyberpunk 2077
                new GamePlatform { GameId = cyberpunk.Id, PlatformId = pcPlatform.Id },
                new GamePlatform { GameId = cyberpunk.Id, PlatformId = ps5Platform.Id },
                new GamePlatform { GameId = cyberpunk.Id, PlatformId = xboxSeriesPlatform.Id },
                new GamePlatform { GameId = cyberpunk.Id, PlatformId = ps4Platform.Id },
                new GamePlatform { GameId = cyberpunk.Id, PlatformId = xboxOnePlatform.Id },
                new GamePlatform { GameId = cyberpunk.Id, PlatformId = stadiaPlatform.Id },
                
                // Zelda Games
                new GamePlatform { GameId = zeldaBotw.Id, PlatformId = switchPlatform.Id },
                new GamePlatform { GameId = zeldaBotw.Id, PlatformId = context.Platforms.First(p => p.Name == "Nintendo Wii U").Id },
                new GamePlatform { GameId = zeldaTotk.Id, PlatformId = switchPlatform.Id },
                
                // Mario Odyssey
                new GamePlatform { GameId = marioOdyssey.Id, PlatformId = switchPlatform.Id },
                
                // Final Fantasy VII Remake
                new GamePlatform { GameId = ff7Remake.Id, PlatformId = ps5Platform.Id },
                new GamePlatform { GameId = ff7Remake.Id, PlatformId = ps4Platform.Id },
                
                // Skyrim
                new GamePlatform { GameId = skyrim.Id, PlatformId = pcPlatform.Id },
                new GamePlatform { GameId = skyrim.Id, PlatformId = ps4Platform.Id },
                new GamePlatform { GameId = skyrim.Id, PlatformId = xboxOnePlatform.Id },
                new GamePlatform { GameId = skyrim.Id, PlatformId = switchPlatform.Id },
                new GamePlatform { GameId = skyrim.Id, PlatformId = ps3Platform.Id },
                new GamePlatform { GameId = skyrim.Id, PlatformId = xbox360Platform.Id },
                
                // Fallout 4
                new GamePlatform { GameId = fallout4.Id, PlatformId = pcPlatform.Id },
                new GamePlatform { GameId = fallout4.Id, PlatformId = ps4Platform.Id },
                new GamePlatform { GameId = fallout4.Id, PlatformId = xboxOnePlatform.Id },
                
                // GTA V
                new GamePlatform { GameId = gta5.Id, PlatformId = pcPlatform.Id },
                new GamePlatform { GameId = gta5.Id, PlatformId = ps5Platform.Id },
                new GamePlatform { GameId = gta5.Id, PlatformId = ps4Platform.Id },
                new GamePlatform { GameId = gta5.Id, PlatformId = xboxSeriesPlatform.Id },
                new GamePlatform { GameId = gta5.Id, PlatformId = xboxOnePlatform.Id },
                new GamePlatform { GameId = gta5.Id, PlatformId = ps3Platform.Id },
                new GamePlatform { GameId = gta5.Id, PlatformId = xbox360Platform.Id },
                
                // Red Dead Redemption 2
                new GamePlatform { GameId = rdr2.Id, PlatformId = pcPlatform.Id },
                new GamePlatform { GameId = rdr2.Id, PlatformId = ps4Platform.Id },
                new GamePlatform { GameId = rdr2.Id, PlatformId = xboxOnePlatform.Id },
                new GamePlatform { GameId = rdr2.Id, PlatformId = stadiaPlatform.Id },
                
                // The Witcher 3
                new GamePlatform { GameId = witcher3.Id, PlatformId = pcPlatform.Id },
                new GamePlatform { GameId = witcher3.Id, PlatformId = ps5Platform.Id },
                new GamePlatform { GameId = witcher3.Id, PlatformId = ps4Platform.Id },
                new GamePlatform { GameId = witcher3.Id, PlatformId = xboxSeriesPlatform.Id },
                new GamePlatform { GameId = witcher3.Id, PlatformId = xboxOnePlatform.Id },
                new GamePlatform { GameId = witcher3.Id, PlatformId = switchPlatform.Id },
                
                // Dark Souls III
                new GamePlatform { GameId = darkSouls3.Id, PlatformId = pcPlatform.Id },
                new GamePlatform { GameId = darkSouls3.Id, PlatformId = ps4Platform.Id },
                new GamePlatform { GameId = darkSouls3.Id, PlatformId = xboxOnePlatform.Id },
                
                // Elden Ring
                new GamePlatform { GameId = eldenRing.Id, PlatformId = pcPlatform.Id },
                new GamePlatform { GameId = eldenRing.Id, PlatformId = ps5Platform.Id },
                new GamePlatform { GameId = eldenRing.Id, PlatformId = ps4Platform.Id },
                new GamePlatform { GameId = eldenRing.Id, PlatformId = xboxSeriesPlatform.Id },
                new GamePlatform { GameId = eldenRing.Id, PlatformId = xboxOnePlatform.Id },
                
                // God of War
                new GamePlatform { GameId = godOfWar.Id, PlatformId = ps4Platform.Id },
                new GamePlatform { GameId = godOfWar.Id, PlatformId = pcPlatform.Id },
                
                // Horizon Zero Dawn
                new GamePlatform { GameId = horizon.Id, PlatformId = ps4Platform.Id },
                new GamePlatform { GameId = horizon.Id, PlatformId = pcPlatform.Id },
                
                // Persona 5 Royal
                new GamePlatform { GameId = persona5.Id, PlatformId = ps5Platform.Id },
                new GamePlatform { GameId = persona5.Id, PlatformId = ps4Platform.Id },
                new GamePlatform { GameId = persona5.Id, PlatformId = xboxSeriesPlatform.Id },
                new GamePlatform { GameId = persona5.Id, PlatformId = xboxOnePlatform.Id },
                new GamePlatform { GameId = persona5.Id, PlatformId = switchPlatform.Id },
                new GamePlatform { GameId = persona5.Id, PlatformId = pcPlatform.Id },
                
                // Hades
                new GamePlatform { GameId = hades.Id, PlatformId = pcPlatform.Id },
                new GamePlatform { GameId = hades.Id, PlatformId = ps5Platform.Id },
                new GamePlatform { GameId = hades.Id, PlatformId = ps4Platform.Id },
                new GamePlatform { GameId = hades.Id, PlatformId = xboxSeriesPlatform.Id },
                new GamePlatform { GameId = hades.Id, PlatformId = xboxOnePlatform.Id },
                new GamePlatform { GameId = hades.Id, PlatformId = switchPlatform.Id },
                
                // Stardew Valley
                new GamePlatform { GameId = stardew.Id, PlatformId = pcPlatform.Id },
                new GamePlatform { GameId = stardew.Id, PlatformId = ps4Platform.Id },
                new GamePlatform { GameId = stardew.Id, PlatformId = xboxOnePlatform.Id },
                new GamePlatform { GameId = stardew.Id, PlatformId = switchPlatform.Id },
                new GamePlatform { GameId = stardew.Id, PlatformId = iosPlatform.Id },
                new GamePlatform { GameId = stardew.Id, PlatformId = androidPlatform.Id },
                
                // Minecraft
                new GamePlatform { GameId = minecraft.Id, PlatformId = pcPlatform.Id },
                new GamePlatform { GameId = minecraft.Id, PlatformId = ps4Platform.Id },
                new GamePlatform { GameId = minecraft.Id, PlatformId = xboxOnePlatform.Id },
                new GamePlatform { GameId = minecraft.Id, PlatformId = switchPlatform.Id },
                new GamePlatform { GameId = minecraft.Id, PlatformId = iosPlatform.Id },
                new GamePlatform { GameId = minecraft.Id, PlatformId = androidPlatform.Id },
                
                // Portal 2
                new GamePlatform { GameId = portal2.Id, PlatformId = pcPlatform.Id },
                new GamePlatform { GameId = portal2.Id, PlatformId = ps3Platform.Id },
                new GamePlatform { GameId = portal2.Id, PlatformId = xbox360Platform.Id },
                
                // RE2 Remake
                new GamePlatform { GameId = re2.Id, PlatformId = pcPlatform.Id },
                new GamePlatform { GameId = re2.Id, PlatformId = ps5Platform.Id },
                new GamePlatform { GameId = re2.Id, PlatformId = ps4Platform.Id },
                new GamePlatform { GameId = re2.Id, PlatformId = xboxSeriesPlatform.Id },
                new GamePlatform { GameId = re2.Id, PlatformId = xboxOnePlatform.Id },
                
                // AC Valhalla
                new GamePlatform { GameId = acValhalla.Id, PlatformId = pcPlatform.Id },
                new GamePlatform { GameId = acValhalla.Id, PlatformId = ps5Platform.Id },
                new GamePlatform { GameId = acValhalla.Id, PlatformId = ps4Platform.Id },
                new GamePlatform { GameId = acValhalla.Id, PlatformId = xboxSeriesPlatform.Id },
                new GamePlatform { GameId = acValhalla.Id, PlatformId = xboxOnePlatform.Id },
                new GamePlatform { GameId = acValhalla.Id, PlatformId = stadiaPlatform.Id },
                
                // Doom Eternal
                new GamePlatform { GameId = doomEternal.Id, PlatformId = pcPlatform.Id },
                new GamePlatform { GameId = doomEternal.Id, PlatformId = ps5Platform.Id },
                new GamePlatform { GameId = doomEternal.Id, PlatformId = ps4Platform.Id },
                new GamePlatform { GameId = doomEternal.Id, PlatformId = xboxSeriesPlatform.Id },
                new GamePlatform { GameId = doomEternal.Id, PlatformId = xboxOnePlatform.Id },
                new GamePlatform { GameId = doomEternal.Id, PlatformId = switchPlatform.Id },
                
                // Animal Crossing
                new GamePlatform { GameId = animalCrossing.Id, PlatformId = switchPlatform.Id },
                
                // PlayStation Exclusives
                new GamePlatform { GameId = lastOfUs2.Id, PlatformId = ps4Platform.Id },
                new GamePlatform { GameId = ghostOfTsushima.Id, PlatformId = ps5Platform.Id },
                new GamePlatform { GameId = ghostOfTsushima.Id, PlatformId = ps4Platform.Id },
                new GamePlatform { GameId = spiderman.Id, PlatformId = ps5Platform.Id },
                new GamePlatform { GameId = spiderman.Id, PlatformId = ps4Platform.Id },
                new GamePlatform { GameId = demonsSouls.Id, PlatformId = ps5Platform.Id },

                // RE Requiem
                new GamePlatform { GameId = reRequiem.Id, PlatformId = pcPlatform.Id },
                new GamePlatform { GameId = reRequiem.Id, PlatformId = ps5Platform.Id },
            };

            context.GamePlatforms.AddRange(gamePlatforms);
            context.SaveChanges();

            // Add GameGenre (Συσχέτιση παιχνιδιών με είδη)
            var actionGenre = context.Genres.First(g => g.Name == "Action");
            var adventureGenre = context.Genres.First(g => g.Name == "Adventure");
            var rpgGenre = context.Genres.First(g => g.Name == "RPG");
            var fpsGenre = context.Genres.First(g => g.Name == "FPS");
            var strategyGenre = context.Genres.First(g => g.Name == "Strategy");
            var simulationGenre = context.Genres.First(g => g.Name == "Simulation");
            var sportsGenre = context.Genres.First(g => g.Name == "Sports");
            var racingGenre = context.Genres.First(g => g.Name == "Racing");
            var horrorGenre = context.Genres.First(g => g.Name == "Horror");
            var survivalGenre = context.Genres.First(g => g.Name == "Survival");
            var openWorldGenre = context.Genres.First(g => g.Name == "Open World");
            var platformerGenre = context.Genres.First(g => g.Name == "Platformer");
            var puzzleGenre = context.Genres.First(g => g.Name == "Puzzle");
            var sandboxGenre = context.Genres.First(g => g.Name == "Sandbox");

            var gameGenres = new List<GameGenre>
            {
                // Zelda BOTW
                new GameGenre { GameId = zeldaBotw.Id, GenreId = actionGenre.Id },
                new GameGenre { GameId = zeldaBotw.Id, GenreId = adventureGenre.Id },
                new GameGenre { GameId = zeldaBotw.Id, GenreId = openWorldGenre.Id },
                
                // Cyberpunk
                new GameGenre { GameId = cyberpunk.Id, GenreId = actionGenre.Id },
                new GameGenre { GameId = cyberpunk.Id, GenreId = rpgGenre.Id },
                new GameGenre { GameId = cyberpunk.Id, GenreId = openWorldGenre.Id },
                
                // Skyrim
                new GameGenre { GameId = skyrim.Id, GenreId = rpgGenre.Id },
                new GameGenre { GameId = skyrim.Id, GenreId = actionGenre.Id },
                new GameGenre { GameId = skyrim.Id, GenreId = openWorldGenre.Id },
                
                // GTA V
                new GameGenre { GameId = gta5.Id, GenreId = actionGenre.Id },
                new GameGenre { GameId = gta5.Id, GenreId = adventureGenre.Id },
                new GameGenre { GameId = gta5.Id, GenreId = openWorldGenre.Id },
                
                // The Witcher 3
                new GameGenre { GameId = witcher3.Id, GenreId = rpgGenre.Id },
                new GameGenre { GameId = witcher3.Id, GenreId = actionGenre.Id },
                new GameGenre { GameId = witcher3.Id, GenreId = openWorldGenre.Id },
                
                // Dark Souls III
                new GameGenre { GameId = darkSouls3.Id, GenreId = rpgGenre.Id },
                new GameGenre { GameId = darkSouls3.Id, GenreId = actionGenre.Id },
                
                // Doom Eternal
                new GameGenre { GameId = doomEternal.Id, GenreId = fpsGenre.Id },
                new GameGenre { GameId = doomEternal.Id, GenreId = actionGenre.Id },
                
                // Portal 2
                new GameGenre { GameId = portal2.Id, GenreId = puzzleGenre.Id },
                new GameGenre { GameId = portal2.Id, GenreId = adventureGenre.Id },
                
                // Minecraft
                new GameGenre { GameId = minecraft.Id, GenreId = sandboxGenre.Id },
                new GameGenre { GameId = minecraft.Id, GenreId = adventureGenre.Id },
                
                // Stardew Valley
                new GameGenre { GameId = stardew.Id, GenreId = simulationGenre.Id },
                new GameGenre { GameId = stardew.Id, GenreId = rpgGenre.Id },
                
                // Resident Evil 2
                new GameGenre { GameId = re2.Id, GenreId = horrorGenre.Id },
                new GameGenre { GameId = re2.Id, GenreId = actionGenre.Id },
                new GameGenre { GameId = re2.Id, GenreId = survivalGenre.Id },

                // Resident Evil Requiem
                new GameGenre { GameId = reRequiem.Id, GenreId = horrorGenre.Id },
                new GameGenre { GameId = reRequiem.Id, GenreId = actionGenre.Id },
                new GameGenre { GameId = reRequiem.Id, GenreId = survivalGenre.Id },
                
                // Super Mario Odyssey
                new GameGenre { GameId = marioOdyssey.Id, GenreId = platformerGenre.Id },
                new GameGenre { GameId = marioOdyssey.Id, GenreId = adventureGenre.Id },
                
                // Animal Crossing
                new GameGenre { GameId = animalCrossing.Id, GenreId = simulationGenre.Id },
                new GameGenre { GameId = animalCrossing.Id, GenreId = adventureGenre.Id }
            };

            context.GameGenres.AddRange(gameGenres);
            context.SaveChanges();

            // Add Users (Εμπλουτισμός με 10+ χρήστες)
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
                    },
                    new User
                    {
                        Username = "gamerpro",
                        Email = "pro@gamer.com",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Gamer123!"),
                        Role = "User"
                    },
                    new User
                    {
                        Username = "reviewer",
                        Email = "review@gamehub.com",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Review123!"),
                        Role = "Moderator"
                    },
                    new User
                    {
                        Username = "collector",
                        Email = "collect@games.com",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Collect123!"),
                        Role = "User"
                    },
                    new User
                    {
                        Username = "retrofan",
                        Email = "retro@classic.com",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Retro123!"),
                        Role = "User"
                    },
                    new User
                    {
                        Username = "indielover",
                        Email = "indie@games.com",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Indie123!"),
                        Role = "User"
                    },
                    new User
                    {
                        Username = "strategist",
                        Email = "strategy@player.com",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Strategy123!"),
                        Role = "User"
                    },
                    new User
                    {
                        Username = "rpgenjoyer",
                        Email = "rpg@fantasy.com",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Rpg123!"),
                        Role = "User"
                    },
                    new User
                    {
                        Username = "speedrunner",
                        Email = "speed@run.com",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Speed123!"),
                        Role = "User"
                    },
                    new User
                    {
                        Username = "moderator",
                        Email = "mod@gamecollection.com",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Mod123!"),
                        Role = "Moderator"
                    }
                };

                context.Users.AddRange(users);
                context.SaveChanges();
            }

            
            var adminUser = context.Users.First(u => u.Username == "admin");
            var testUser = context.Users.First(u => u.Username == "testuser");
            var gamerPro = context.Users.First(u => u.Username == "gamerpro");
            var reviewer = context.Users.First(u => u.Username == "reviewer");
            var collector = context.Users.First(u => u.Username == "collector");

            context.SaveChanges();

            // Add Collections (Συλλογές χρηστών)
            var collections = new[]
            {
                new GameCollection.Domain.Entities.GameCollection
                {
                    UserId = adminUser.Id,
                    Name = "Must Play Games",
                    Description = "Τα απαραίτητα παιχνίδια που πρέπει να παίξει κανείς."
                },
                new GameCollection.Domain.Entities.GameCollection
                {
                    UserId = adminUser.Id,
                    Name = "RPG Masterpieces",
                    Description = "Τα καλύτερα RPG όλων των εποχών."
                },
                new GameCollection.Domain.Entities.GameCollection
                {
                    UserId = testUser.Id,
                    Name = "Open World Adventures",
                    Description = "Μεγάλα open world για εξερεύνηση."
                },
                new GameCollection.Domain.Entities.GameCollection
                {
                    UserId = testUser.Id,
                    Name = "My Favorites",
                    Description = "Τα αγαπημένα μου παιχνίδια."
                },
                new GameCollection.Domain.Entities.GameCollection
                {
                    UserId = gamerPro.Id,
                    Name = "Challenging Games",
                    Description = "Παιχνίδια που θέλουν skill και υπομονή."
                },
                new GameCollection.Domain.Entities.GameCollection
                {
                    UserId = reviewer.Id,
                    Name = "Game of the Year Candidates",
                    Description = "Παιχνίδια που άξιζαν GOTY."
                },
                new GameCollection.Domain.Entities.GameCollection
                {
                    UserId = collector.Id,
                    Name = "Indie Gems",
                    Description = "Κρυμμένα διαμάντια από indie developers."
                }
            };

            context.GameCollections.AddRange(collections);
            context.SaveChanges();

            // Add CollectionGames (Παιχνίδια στις συλλογές)
            var mustPlayCollection = collections.First(c => c.Name == "Must Play Games");
            var rpgCollection = collections.First(c => c.Name == "RPG Masterpieces");
            var openWorldCollection = collections.First(c => c.Name == "Open World Adventures");
            var favoritesCollection = collections.First(c => c.Name == "My Favorites");
            var challengingCollection = collections.First(c => c.Name == "Challenging Games");
            var gotyCollection = collections.First(c => c.Name == "Game of the Year Candidates");
            var indieCollection = collections.First(c => c.Name == "Indie Gems");

            var collectionGames = new[]
            {
                // Must Play Collection
                new CollectionGame { CollectionId = mustPlayCollection.Id, GameId = zeldaBotw.Id},
                new CollectionGame { CollectionId = mustPlayCollection.Id, GameId = witcher3.Id},
                new CollectionGame { CollectionId = mustPlayCollection.Id, GameId = portal2.Id},
                new CollectionGame { CollectionId = mustPlayCollection.Id, GameId = gta5.Id},
                
                // RPG Masterpieces
                new CollectionGame { CollectionId = rpgCollection.Id, GameId = witcher3.Id},
                new CollectionGame { CollectionId = rpgCollection.Id, GameId = persona5.Id},
                new CollectionGame { CollectionId = rpgCollection.Id, GameId = skyrim.Id},
                new CollectionGame { CollectionId = rpgCollection.Id, GameId = eldenRing.Id},
                
                // Open World Adventures
                new CollectionGame { CollectionId = openWorldCollection.Id, GameId = zeldaBotw.Id},
                new CollectionGame { CollectionId = openWorldCollection.Id, GameId = gta5.Id},
                new CollectionGame { CollectionId = openWorldCollection.Id, GameId = rdr2.Id},
                new CollectionGame { CollectionId = openWorldCollection.Id, GameId = cyberpunk.Id},
                
                // My Favorites
                new CollectionGame { CollectionId = favoritesCollection.Id, GameId = cyberpunk.Id},
                new CollectionGame { CollectionId = favoritesCollection.Id, GameId = doomEternal.Id},
                
                // Challenging Games
                new CollectionGame { CollectionId = challengingCollection.Id, GameId = darkSouls3.Id},
                new CollectionGame { CollectionId = challengingCollection.Id, GameId = eldenRing.Id},
                
                // GOTY Candidates
                new CollectionGame { CollectionId = gotyCollection.Id, GameId = eldenRing.Id},
                new CollectionGame { CollectionId = gotyCollection.Id, GameId = godOfWar.Id},
                new CollectionGame { CollectionId = gotyCollection.Id, GameId = lastOfUs2.Id},
                new CollectionGame { CollectionId = gotyCollection.Id, GameId = hades.Id},
                
                // Indie Gems
                new CollectionGame { CollectionId = indieCollection.Id, GameId = hades.Id},
                new CollectionGame { CollectionId = indieCollection.Id, GameId = stardew.Id}
            };

            context.CollectionGames.AddRange(collectionGames);
            context.SaveChanges();

            Console.WriteLine("Seed data added successfully!");
        }
    }
}