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

        public UserProfileDto UserProfile { get; set; }
        public int TotalCollections { get; set; }
        public int TotalGamesInCollections { get; set; }

        public ProfileModel(IUserService userService, ICurrentUserService currentUser)
        {
            _userService = userService;
            _currentUser = currentUser;
        }

        public UserStatsDto UserStats { get; set; }

        public async Task<IActionResult> OnGetAsync()
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
