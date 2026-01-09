using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GameCollection.Domain.Entities;

namespace GameCollection.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { 
        }

        public DbSet<Game> Games { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Platform> Platforms { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Franchise> Franchises { get; set; }
        public DbSet<GameCollection.Domain.Entities.GameCollection> GameCollections { get; set; }
        public DbSet<CollectionGame> CollectionGames { get; set; }
        public DbSet<GamePlatform> GamePlatforms { get; set; }
        public DbSet<GameGenre> GameGenres { get; set; }
        public DbSet<UserReview> UserReviews { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Each Entity
            ConfigureGame(modelBuilder);
            ConfigureUser(modelBuilder);
            ConfigurePlatform(modelBuilder);
            ConfigureGenre(modelBuilder);
            ConfigureFranchise(modelBuilder);
            ConfigureGameCollection(modelBuilder);
            ConfigureJunctionTables(modelBuilder);
            ConfigureUserReview(modelBuilder);
        }

        private void ConfigureGame(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>(entity =>
            {
                entity.HasKey(g => g.Id);

                entity.Property(g => g.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(g => g.Description)
                    .HasMaxLength(1000);

                entity.Property(g => g.Developer)
                    .HasMaxLength(100);

                entity.Property(g => g.Publisher)
                    .HasMaxLength(100);

                entity.Property(g => g.ReleaseYear)
                    .IsRequired();

                // Indexes
                entity.HasIndex(g => g.Title);
                entity.HasIndex(g => g.ReleaseYear);
                entity.HasIndex(g => g.MetacriticScore);

                // Franchise Relation
                entity.HasOne(g => g.Franchise)
                    .WithMany(f => f.Games)
                    .HasForeignKey(g => g.FranchiseId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private void ConfigureUser(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);

                entity.Property(u => u.Username)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(u => u.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                // Indexes
                entity.HasIndex(u => u.Username).IsUnique();
                entity.HasIndex(u => u.Email).IsUnique();

                // Collections Relation
                entity.HasMany(u => u.Collections)
                    .WithOne(c => c.User)
                    .HasForeignKey(c => c.UserId)                           
                    .OnDelete(DeleteBehavior.Cascade);

                // Reviews Relation
                entity.HasMany(u => u.Reviews)
                    .WithOne(r => r.User)
                    .HasForeignKey(r => r.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        private void ConfigurePlatform(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Platform>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.Property(p => p.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasIndex(p => p.Name).IsUnique();
            });
        }

        private void ConfigureGenre(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Genre>(entity =>
            {
                entity.HasKey(g => g.Id);

                entity.Property(g => g.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasIndex(g => g.Name).IsUnique();
            });
        }

        private void ConfigureFranchise(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Franchise>(entity =>
            {
                entity.HasKey(f => f.Id);

                entity.Property(f => f.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasIndex(f => f.Name).IsUnique();
            });
        }

        private void ConfigureGameCollection(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameCollection.Domain.Entities.GameCollection>(entity =>
            {
                entity.HasKey(gc => gc.Id);

                entity.Property(gc => gc.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(gc => gc.Description)
                    .HasMaxLength(500);

                // User Relation
                entity.HasOne(gc => gc.User)
                    .WithMany(u => u.Collections)
                    .HasForeignKey(gc => gc.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        private void ConfigureJunctionTables(ModelBuilder modelBuilder)
        {
            // Collection Game
            modelBuilder.Entity<CollectionGame>(entity =>
            {
                // Primary Key
                entity.HasKey(cg => new { cg.CollectionId, cg.GameId});

                entity.Property(cg => cg.PersonalNotes)
                    .HasMaxLength(1000);

                entity.HasOne(cg => cg.Collection)
                    .WithMany(c => c.CollectionGames)
                    .HasForeignKey(cg => cg.CollectionId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(cg => cg.Game)
                    .WithMany(g => g.CollectionGames)
                    .HasForeignKey(cg => cg.GameId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(cg => cg.DateAdded);
            });

            // Game Platform
            modelBuilder.Entity<GamePlatform>(entity =>
            {
                entity.HasKey(gp => new { gp.GameId, gp.PlatformId });

                entity.HasOne(gp => gp.Game)
                    .WithMany(g => g.GamePlatforms)
                    .HasForeignKey(gp => gp.GameId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(gp => gp.Platform)
                    .WithMany(p => p.GamePlatforms)
                    .HasForeignKey(gp => gp.PlatformId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Game Genre
            modelBuilder.Entity<GameGenre>(entity =>
            {
                entity.HasKey(gg => new { gg.GameId, gg.GenreId });

                entity.HasOne(gg => gg.Game)
                    .WithMany(g => g.GameGenres)
                    .HasForeignKey(gg => gg.GameId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(gg => gg.Genre)
                    .WithMany(g => g.GameGenres)
                    .HasForeignKey(gg => gg.GenreId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        private void ConfigureUserReview(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserReview>(entity =>
            {
                entity.HasKey(ur => ur.Id);

                entity.Property(ur => ur.ReviewText)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.Property(ur => ur.Rating)
                    .IsRequired();

                entity.HasIndex(ur => new { ur.UserId, ur.GameId }).IsUnique();

                entity.HasOne(ur => ur.User)
                    .WithMany(u => u.Reviews)
                    .HasForeignKey(ur => ur.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ur => ur.Game)
                    .WithMany(g => g.UserReviews)
                    .HasForeignKey(ur => ur.GameId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
