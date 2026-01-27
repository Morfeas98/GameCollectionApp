using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCollection.Application.DTOs
{
    public class UserStatsDto
    {
        public int TotalCollections { get; set; }
        public int TotalGamesInCollections { get; set; }
        public int TotalReviews { get; set; }
        public double AverageRating { get; set; }
        public int CurrentlyPlaying { get; set; }
        public int CompletedGames { get; set; }
        public int WishlistCount { get; set; }
        public DateTime? LastActivity { get; set; }
    }
}
