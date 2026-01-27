using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GameCollection.API.ViewModels
{
    public class GameFormViewModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Τίτλος υποχρεωτικός")]
        [MaxLength(200, ErrorMessage = "Μέγιστο όριο 200 χαρακτήρες")]
        [Display(Name = "Τίτλος Παιχνιδιού")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Έτος κυκλοφορίας υποχρεωτικό")]
        [Range(1950, 2100, ErrorMessage = "Έτος κυκλοφορίας μεταξύ 1950 και 2100")]
        [Display(Name = "Έτος κυκλοφορίας")]
        public int ReleaseYear { get; set; } = DateTime.Now.Year;

        [MaxLength(1000, ErrorMessage = "Μέγιστο όριο 1000 χαρακτήρες")]
        [Display(Name = "Περιγραφή")]
        public string? Description { get; set; }

        [MaxLength(100, ErrorMessage = "Μέγιστο όριο 100 χαρακτήρες")]
        [Display(Name = "Δημιουργός")]
        public string? Developer { get; set; }

        [MaxLength(100, ErrorMessage = "Μέγιστο όριο 100 χαρακτήρες")]
        [Display(Name = "Εκδότης")]
        public string? Publisher { get; set; }

        [Url(ErrorMessage = "Πρέπει να είναι έγκυρο Url")]
        [Display(Name = "Εικόνα (URL)")]
        public string? ImageUrl { get; set; }

        [Range(0, 100, ErrorMessage = "Τιμή μεταξύ 0 και 100")]
        [Display(Name = "Metacritic Score")]
        public int? MetacriticScore { get; set; }

        [Url(ErrorMessage = "Πρέπει να είναι έγκυρο Url")]
        [Display(Name = "Metacritic URL")]
        public string? MetacriticUrl { get; set; }

        [Display(Name = "Franchise")]
        public int? FranchiseId { get; set; }

        [Display(Name = "Πλατφόρμες")]
        public List<int> SelectedPlatformIds { get; set; } = new();

        [Display(Name = "Είδη")]
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
