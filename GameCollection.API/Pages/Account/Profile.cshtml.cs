using GameCollection.Application.DTOs;
using GameCollection.Application.Services;
using GameCollection.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace GameCollection.API.Pages.Account
{
    [Authorize]
    public class ProfileModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly ICurrentUserService _currentUser;
        private readonly ICollectionService _collectionService;

        public UserProfileDto UserProfile { get; set; }
        public int TotalCollections { get; set; }
        public int TotalGamesInCollections { get; set; }
        public UserStatsDto UserStats { get; set; }
        public List<ActivityDto> RecentActivities { get; set; } = new();
        public List<CollectionGameDto> RecentNotes { get; set; } = new();
        public List<CollectionDto> RecentCollections { get; set; } = new();

        public ProfileModel(IUserService userService, ICurrentUserService currentUser, ICollectionService collectionService)
        {
            _userService = userService;
            _currentUser = currentUser;
            _collectionService = collectionService;
        }

        

        public async Task<IActionResult> OnGetAsync(bool? passwordChanged)
        {
            if (!_currentUser.IsAuthenticated)
            {
                TempData["ErrorMessage"] = "Please login to view your profile.";
                return RedirectToPage("/Account/Login", new { returnUrl = "/Account/Profile" });
            }

            try
            {
                var userId = _currentUser.GetRequiredUserId();
                UserProfile = await _userService.GetUserProfileAsync(userId);

                var stats = await _userService.GetUserStatsAsync(userId);
                TotalCollections = stats.TotalCollections;
                TotalGamesInCollections = stats.TotalGamesInCollections;
                UserStats = stats;

                RecentActivities = await _userService.GetRecentActivitiesAsync(userId, 5);
                RecentNotes = await _userService.GetRecentNotesAsync(userId, 5);

                var allCollections = await _collectionService.GetUserCollectionsAsync(userId);
                RecentCollections = allCollections?
                    .OrderByDescending(c => c.CreatedAt)
                    .Take(3)
                    .ToList() ?? new List<CollectionDto>();

                if (passwordChanged.HasValue && passwordChanged.Value)
                {
                    TempData["SuccessMessage"] = "Password changed successfully!";
                }

                return Page();
            }
            catch (KeyNotFoundException)
            {
                TempData["ErrorMessage"] = "User profile not found.";
                return RedirectToPage("/Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                return RedirectToPage("/Index");
            }
        }

    }
}
