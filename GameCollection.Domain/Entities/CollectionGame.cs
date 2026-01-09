using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using GameCollection.Domain.Common;

namespace GameCollection.Domain.Entities
{
    public class CollectionGame : BaseEntity
    {
        // Foreign Keys
        public int CollectionId { get; set; }
        public int GameId { get; set; }

        // Additional Data
        public DateTime DateAdded { get; set; } = DateTime.UtcNow;

        [Range(1, 10)]
        public int? PersonalRating { get; set; }

        [MaxLength(1000)]
        public string? PersonalNotes { get; set; }

        public bool Completed { get; set; }
        public bool CurrentlyPlaying { get; set; }

        // Navigation
        public virtual GameCollection Collection { get; set; }
        public virtual Game Game { get; set; }
    }
}
