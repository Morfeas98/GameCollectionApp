using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using GameCollection.Domain.Common;

namespace GameCollection.Domain.Entities
{
    public class Franchise : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Url]
        public string? LogoUrl { get; set; }

        // Navigation
        public virtual ICollection<Game> Games { get; set; } = new HashSet<Game>();
    }
}
