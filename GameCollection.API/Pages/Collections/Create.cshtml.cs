using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GameCollection.Application.DTOs;
using GameCollection.Application.Services;

namespace GameCollection.API.Pages.Collections
{
    public class CreateModel : PageModel
    {
        private readonly ICollectionService _collectionService;
        private readonly ICurrentUserService _currentUser;

        [BindProperty]
        public CreateCollectionDto CreateCollectionDto { get; set; } = new();

        public CreateModel(
            ICollectionService collectionService,
            ICurrentUserService currentUser)
        {
            _collectionService = collectionService;
            _currentUser = currentUser;
        }

        public IActionResult OnGet()
        {
            if (!_currentUser.IsAuthenticated)
            {
                TempData["ErrorMessage"] = "Please login to create a collection.";
                return RedirectToPage("/Account/Login", new { returnUrl = "/Collections/Create" });
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!_currentUser.IsAuthenticated)
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var userId = _currentUser.GetRequiredUserId();
                var createdCollection = await _collectionService.CreateCollectionAsync(CreateCollectionDto, userId);

                TempData["SuccessMessage"] = $"Collection '{createdCollection.Name}' created successfully!";

                return RedirectToPage("Details", new { id = createdCollection.Id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}
