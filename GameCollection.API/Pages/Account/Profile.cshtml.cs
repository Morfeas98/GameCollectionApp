using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GameCollection.Application.DTOs;
using GameCollection.Application.Services;
using System.Security.Claims;

namespace GameCollection.API.Pages.Account
{
    [Authorize]
    public class ProfileModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly ICollectionService _collectionService;

        public UserProfileDto Profile { get; set; }
        public IEnumerable<CollectionDto> Collections { get; set; }

        public ProfileModel(IUserService userService, ICollectionService collectionService)
        {
            _userService = userService;
            _collectionService = collectionService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            try
            {
                Profile = await _userService.GetUserProfileAsync(userId);
                Collections = await _collectionService.GetUserCollectionsAsync(userId);

                return Page();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
