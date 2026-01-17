using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace GameCollection.Application.DTOs
{
    public class CollectionDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public int GameCount { get; set; }
        public List<CollectionGameDto>? Games { get; set; }
    }

    public class CreateCollectionDto
    {
        [Required(ErrorMessage = "Collection name is required")]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }
    }

    public class UpdateCollectionDto
    {
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string? Name { get; set; }

        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }
    }

    public class CollectionGameDto
    {
        public int GameId { get; set; }
        public string GameTitle { get; set; } = string.Empty;
        public DateTime DateAdded { get; set; }
        public int? PersonalRating { get; set; }
        public string? PersonalNotes { get; set; }
        public bool Completed { get; set; }
        public bool CurrentlyPlaying { get; set; }
    }

    public class AddGameToCollectionDto
    {
        [Required(ErrorMessage = "Game ID is required")]
        public int GameId { get; set; }

        [Range(1, 10, ErrorMessage = "Rating must be between 1 and 10")]
        public int? PersonalRating { get; set; }

        [MaxLength(1000, ErrorMessage = "Notes cannot exceed 1000 characters")]
        public string? PersonalNotes { get; set; }
    }
}
