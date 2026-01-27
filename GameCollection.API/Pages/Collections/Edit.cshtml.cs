using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GameCollection.Application.Services;
using GameCollection.Application.DTOs;

namespace GameCollection.API.Pages.Collections
{
    public class EditModel : PageModel
    {
        private readonly ICollectionService _collectionService;
        private readonly ICurrentUserService _currentUser; 

        [BindProperty]
        public UpdateCollectionDto UpdateCollectionDto { get; set; } = new();

        public CollectionDto CurrentCollection { get; set; }

        public EditModel(
            ICollectionService collectionService,
            ICurrentUserService currentUser)
        {
            _collectionService = collectionService;
            _currentUser = currentUser; 
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (!_currentUser.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login", new { returnUrl = $"/Collections/Edit/{id}" });
            }

            var userId = _currentUser.GetRequiredUserId();
            CurrentCollection = await _collectionService.GetCollectionByIdAsync(id, userId);

            if (CurrentCollection == null)
            {
                return NotFound();
            }

            UpdateCollectionDto.Name = CurrentCollection.Name;
            UpdateCollectionDto.Description = CurrentCollection.Description;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!_currentUser.IsAuthenticated)
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                await ReloadCurrentCollection(id);
                return Page();
            }

            try
            {
                var userId = _currentUser.GetRequiredUserId();
                var updatedCollection = await _collectionService.UpdateCollectionAsync(id, UpdateCollectionDto, userId);

                if (updatedCollection == null)
                {
                    return NotFound();
                }

                TempData["SuccessMessage"] = $"Collection '{updatedCollection.Name}' updated successfully!";

                return RedirectToPage("Details", new { id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await ReloadCurrentCollection(id);
                return Page();
            }
        }

        private async Task ReloadCurrentCollection(int id)
        {
            if (_currentUser.IsAuthenticated)
            {
                var userId = _currentUser.GetRequiredUserId();
                CurrentCollection = await _collectionService.GetCollectionByIdAsync(id, userId);
            }
        }
    }
}
