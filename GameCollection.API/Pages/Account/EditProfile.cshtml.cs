using GameCollection.Application.DTOs;
using GameCollection.Application.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace GameCollection.API.Pages.Account
{
    [Authorize]
    public class EditProfileModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly ICurrentUserService _currentUser;

        [BindProperty]
        public UserProfileUpdateDto UpdateProfile { get; set; } = new();

        public UserProfileDto? CurrentProfile { get; set; }

        [TempData]
        public string? SuccessMessage { get; set; }

        [TempData]
        public string? ErrorMessage { get; set; }

        public EditProfileModel(IUserService userService, ICurrentUserService currentUser)
        {
            _userService = userService;
            _currentUser = currentUser;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var userId = _currentUser.GetRequiredUserId();
                CurrentProfile = await _userService.GetUserProfileAsync(userId);

                UpdateProfile.Username = CurrentProfile.Username;
                UpdateProfile.Email = CurrentProfile.Email;

                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error loading profile: {ex.Message}";
                return RedirectToPage("/Account/Profile");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var userId = _currentUser.GetRequiredUserId();
                CurrentProfile = await _userService.GetUserProfileAsync(userId);
                return Page();
            }

            try
            {
                var userId = _currentUser.GetRequiredUserId();
                var updatedProfile = await _userService.UpdateUserProfileAsync(userId, UpdateProfile);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                    new Claim(ClaimTypes.Name, updatedProfile.Username), 
                    new Claim(ClaimTypes.Email, updatedProfile.Email),
                    new Claim("Token", User.FindFirstValue("Token") ?? string.Empty)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    new AuthenticationProperties
                {
                        IsPersistent = true, 
                        AllowRefresh = true
                });

                SuccessMessage = "Profile updated successfully!";

                return RedirectToPage("/Account/Profile");
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An unexpected error occurred: {ex.Message}");
            }

            var userIdReload = _currentUser.GetRequiredUserId();
            CurrentProfile = await _userService.GetUserProfileAsync(userIdReload);
            return Page();
        }
    }
}