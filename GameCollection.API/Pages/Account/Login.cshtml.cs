using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GameCollection.Application.Services;
using GameCollection.Application.DTOs;
using System.Security.Claims;

namespace GameCollection.API.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IUserService _userService;

        [BindProperty]
        public LoginDto LoginDto { get; set; } = new();

        [BindProperty]
        public bool RememberMe { get; set; }

        public string? ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        [FromQuery(Name = "errorMessage")]
        public string QueryErrorMessage { get; set; }

        public LoginModel(IUserService userService)
        {
            _userService = userService;
        }

        public void OnGet(string? returnUrl = null)
        {
            ReturnUrl = returnUrl;

            if (!string.IsNullOrEmpty(QueryErrorMessage))
            {
                ErrorMessage = QueryErrorMessage;
            }
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;

            try
            {
                // Authenticate user
                var authResponse = await _userService.LoginAsync(LoginDto);

                // Create claims
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, authResponse.UserId.ToString()),
                    new Claim(ClaimTypes.Name, authResponse.Username),
                    new Claim(ClaimTypes.Email, authResponse.Email),
                    new Claim("Token", authResponse.Token) // Store token in claim
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = RememberMe,
                    ExpiresUtc = RememberMe ? DateTimeOffset.UtcNow.AddDays(7) : null,
                    RedirectUri = ReturnUrl ?? "/"
                };

                // Sign in
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                // Store token in session for API calls
                HttpContext.Session.SetString("JWT_Token", authResponse.Token);

                TempData["SuccessMessage"] = "Login successful! Welcome back.";

                // Redirect
                if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                {
                    return LocalRedirect(ReturnUrl);
                }

                return RedirectToPage("/Index");
            }
            catch (UnauthorizedAccessException ex)
            {
                ErrorMessage = ex.Message;
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
            catch (Exception)
            {
                ErrorMessage = "An error occurred during login. Please try again.";
                ModelState.AddModelError(string.Empty, "An error occurred during login. Please try again.");
                return Page();
            }
        }
    }
}