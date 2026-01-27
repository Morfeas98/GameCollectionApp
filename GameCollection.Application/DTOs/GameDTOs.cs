using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCollection.Application.DTOs
{
    public class GameDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int ReleaseYear { get; set; }
        public string? Developer { get; set; }
        public string? Publisher { get; set; }
        public string? ImageUrl { get; set; }
        public int? MetacriticScore { get; set; }
        public string? MetacriticUrl { get; set; }
        public string? FranchiseName { get; set; }
        public List<string> Platforms { get; set; } = new();
        public List<string> Genres { get; set; } = new();
    }

    public class CreateGameDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int ReleaseYear { get; set; }
        public string? Developer { get; set; }
        public string? Publisher { get; set; }
        public List<int> PlatformIds { get; set; } = new();
        public List<int> GenreIds { get; set; } = new();
        public int? FranchiseId { get; set; }
        public string? ImageUrl { get; set; }
        public int? MetacriticScore { get; set; }
        public string? MetacriticUrl { get; set; }
    }

    public class UpdateGameDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? ReleaseYear { get; set; }
        public string? Developer { get; set; }
        public string? Publisher { get; set; }
        public string? ImageUrl { get; set; }
        public int? MetacriticScore { get; set; }
        public string? MetacriticUrl { get; set; }
        public List<int>? PlatformIds { get; set; }
        public List<int>? GenreIds { get; set; }
        public int? FranchiseId { get; set; }
    }
}
