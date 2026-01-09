using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using GameCollection.Domain.Common;

namespace GameCollection.Domain.Entities
{
    public class Genre : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        // Navigation
        public virtual ICollection<GameGenre> GameGenres { get; set; } = new HashSet<GameGenre>();
    }
}
