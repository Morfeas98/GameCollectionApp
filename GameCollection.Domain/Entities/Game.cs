using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using GameCollection.Domain.Common;

namespace GameCollection.Domain.Entities
{
    public class Game : BaseEntity
    {
        // Mandatory Values
        [Required(ErrorMessage = "Ο Τίτλος είναι υποχρεωτικός!")]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required(ErrorMessage = "To Έτος Κυκλοφορίες είναι υποχρεωτικό!")]
        [Range(1950, 2100)]
        public int ReleaseYear { get; set; }

        // Optional Values
        [MaxLength(1000)]
        public string? Description { get; set; }

        [MaxLength(100)]
        public string? Developer { get; set; }

        [MaxLength(100)]
        public string? Publisher { get; set; }

        [Url]
        public string? ImageUrl { get; set; }

        [Range(0, 100)]
        public int? MetacriticScore { get; set; }

        [Url]
        public string? MetacriticUrl { get; set; }

        // Relationships
        public int? FranchiseId { get; set; }
        public virtual Franchise? Franchise { get; set; }
        public virtual ICollection<GamePlatform> GamePlatforms { get; set; } = new HashSet<GamePlatform>();
        public virtual ICollection<GameGenre> GameGenres { get; set; } = new HashSet<GameGenre>();
        public virtual ICollection<CollectionGame> CollectionGames { get; set; } = new HashSet<CollectionGame>();
        public virtual ICollection<UserReview> UserReviews { get; set; } = new HashSet<UserReview>();
    }
}
