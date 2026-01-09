using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using GameCollection.Domain.Common;

namespace GameCollection.Domain.Entities
{
    public class UserReview : BaseEntity
    {
        // Foreign Keys
        public int UserId { get; set; }
        public int GameId { get; set; }

        // Review Data
        [Required]
        [Range(1, 10)]
        public int Rating { get; set; }

        [Required]
        [MaxLength(2000)]
        public string ReviewText { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation
        public virtual User User { get; set; }
        public virtual Game Game { get; set; }
    }
}
