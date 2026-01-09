using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using GameCollection.Domain.Common;

namespace GameCollection.Domain.Entities
{
    public class Platform : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string? Manufacturer { get; set; }

        [MaxLength(4)]
        public string? ReleaseYear { get; set; }

        public string? Description { get; set; }

        // Navigation
        public virtual ICollection<GamePlatform> GamePlatforms { get; set; } = new HashSet<GamePlatform>();
    }
}
