using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GameCollection.API.ViewModels
{
    public class GameFormViewModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Title is mandatory")]
        [MaxLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
        [Display(Name = "Game Title")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Release Year")]
        [Range(1950, 2100, ErrorMessage = "Release Year must be between 1950 and 2100")]
        [Display(Name = "Release Year")]
        public int ReleaseYear { get; set; } = DateTime.Now.Year;

        [MaxLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        [MaxLength(100, ErrorMessage = "Developer cannot exceed 100 characters")]
        [Display(Name = "Developer")]
        public string? Developer { get; set; }

        [MaxLength(100, ErrorMessage = "Publisher cannot exceed 100 characters")]
        [Display(Name = "Publisher")]
        public string? Publisher { get; set; }

        [Url(ErrorMessage = "Must be a valid Url")]
        [Display(Name = "Image (URL)")]
        public string? ImageUrl { get; set; }

        [Range(0, 100, ErrorMessage = "Score between 0 and 100")]
        [Display(Name = "Metacritic Score")]
        public int? MetacriticScore { get; set; }

        [Url(ErrorMessage = "Must be a valid Url")]
        [Display(Name = "Metacritic URL")]
        public string? MetacriticUrl { get; set; }

        [Display(Name = "Franchise")]
        public int? FranchiseId { get; set; }

        [Display(Name = "Platforms")]
        public List<int> SelectedPlatformIds { get; set; } = new();

        [Display(Name = "Genre")]
        public List<int> SelectedGenreIds { get; set; } = new();

        public List<SelectListItem> Franchises { get; set; } = new();
        public List<SelectListItem> Platforms { get; set; } = new();
        public List<SelectListItem> Genres { get; set; } = new();
    }

    public class DropdownItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
