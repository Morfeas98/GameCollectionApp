using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace GameCollection.Application.DTOs
{
    public class GameQueryParams
    {
        public string? SearchQuery { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Invalid Platform ID")]
        public int? PlatformId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Invalid Genre ID")]
        public int? GenreId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Invalid Franchise ID")]
        public int? FranchiseId { get; set; }

        [Range(1950, 2100, ErrorMessage = "Year must be between 1950 and 2100")]
        public int? MinYear { get; set; }

        [Range(1950, 2100, ErrorMessage = "Year must be between 1950 and 2100")]
        public int? MaxYear { get; set; }

        public string SortBy { get; set; } = "title_asc";

        [Range(1, 100, ErrorMessage = "Page number must be between 1 and 100")]
        public int PageNumber { get; set; } = 1;

        [Range(1, 50, ErrorMessage = "Page size must be between 1 and 50")]
        public int PageSize { get; set; } = 12;

        public bool IncludeDeleted { get; set; } = false;

        // Helper Methods
        public bool HasFilters()
        {
            return !string.IsNullOrEmpty(SearchQuery) ||
                    PlatformId.HasValue ||
                    GenreId.HasValue ||
                    FranchiseId.HasValue ||
                    MinYear.HasValue ||
                    MaxYear.HasValue;
        }

        public string GetSortDisplayName()
        {
            return SortBy switch
            {
                "title_asc" => "Title (A-Z)",
                "title_desc" => "Title (Z-A)",
                "year_asc" => "Year (Oldest First)",
                "year_desc" => "Year (Newest First)",
                "rating_asc" => "Rating (Lowest First)",
                "rating_desc" => "Rating (Highest First)",
                _ => "Title (A-Z)"
            };
        }
    }
}
