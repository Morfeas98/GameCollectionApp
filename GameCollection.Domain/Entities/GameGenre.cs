using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameCollection.Domain.Common;

namespace GameCollection.Domain.Entities
{
    public class GameGenre : BaseEntity
    {
        // Foreign Keys
        public int GameId { get; set; }
        public int GenreId { get; set; }

        // Navigation
        public virtual Game Game { get; set; }
        public virtual Genre Genre { get; set; }
    }
}
