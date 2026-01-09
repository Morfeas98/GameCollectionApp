using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using GameCollection.Domain.Common;

namespace GameCollection.Domain.Entities
{
    public class GameCollection : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        // Foreign Key
        public int UserId { get; set; }

        // Navigation
        public virtual User User { get; set; }
        public virtual ICollection<CollectionGame> CollectionGames { get; set; } = new HashSet<CollectionGame>();
    }
}
