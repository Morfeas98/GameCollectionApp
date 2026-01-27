using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using GameCollection.Application.Services;
using GameCollection.Application.DTOs;

namespace GameCollection.API.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly IUserService _userService;

        [BindProperty]
        public RegisterDto RegisterDto { get; set; } = new();

        [BindProperty]
        public bool AcceptTerms { get; set; }

        public string? ReturnUrl { get; set; }

        public RegisterModel(IUserService userService)
        {
            _userService = userService;
        }

        public void OnGet(string? returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (!AcceptTerms)
            {
                ModelState.AddModelError(string.Empty, "You must accept the terms and conditions");
                return Page();
            }

            try
            {
                // Register User
                var authResponse = await _userService.RegisterAsync(RegisterDto);

                // Create Claims
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, authResponse.UserId.ToString()),
                    new Claim(ClaimTypes.Name, authResponse.Username),
                    new Claim(ClaimTypes.Email, authResponse.Email),
                    new Claim("Token", authResponse.Token)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties()
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7),
                    RedirectUri = ReturnUrl ?? "/"
                };

                // Auto Sign-In after Registration
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                // Store Token in Session
                HttpContext.Session.SetString("JWT Token", authResponse.Token);

                TempData["SuccessMessage"] = $"Welcome {authResponse.Username}! Your account has been created successfully!";

                // Redirect
                if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                {
                    return Redirect(ReturnUrl);
                }

                return RedirectToPage("/Index");
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred during registration.Please try again.");
                return Page();
            }
        }
    }
}
