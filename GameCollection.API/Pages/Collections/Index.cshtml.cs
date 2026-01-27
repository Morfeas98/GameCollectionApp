using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GameCollection.Application.Services;
using GameCollection.Application.DTOs;

namespace GameCollection.API.Pages.Collections
{
    public class IndexModel : PageModel
    {
        private readonly ICollectionService _collectionService;
        private readonly ICurrentUserService _currentUser;

        public List<CollectionDto> UserCollections { get; set; } = new();

        public IndexModel(
            ICollectionService collectionService,
            ICurrentUserService currentUser)
        {
            _collectionService = collectionService;
            _currentUser = currentUser;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!_currentUser.IsAuthenticated)
            {
                TempData["ErrorMessage"] = "Please login to view your collections.";
                return RedirectToPage("/Account/Login", new { returnUrl = "/Collections" });
            }

            var userId = _currentUser.GetRequiredUserId();
            UserCollections = (await _collectionService.GetUserCollectionsAsync(userId)).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            if (!_currentUser.IsAuthenticated)
            {
                return Unauthorized();
            }

            var userId = _currentUser.GetRequiredUserId();
            var result = await _collectionService.DeleteCollectionAsync(id, userId);

            if (result)
            {
                TempData["SuccessMessage"] = "Collection deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete collection or collection not found.";
            }

            return RedirectToPage();
        }
    }
}
