using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using GameCollection.Application.Services;

namespace GameCollection.API.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public LoginModel(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Please enter your username or email")]
            [Display(Name = "Username or Email")]
            public string UsernameOrEmail { get; set; }

            [Required(ErrorMessage = "Please enter your password")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me")]
            public bool RememberMe { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                // Login with IUSerService
                var loginDto = new Application.DTOs.LoginDto()
                {
                    UsernameOrEmail = Input.UsernameOrEmail,
                    Password = Input.Password
                };

                var authResponse = await _userService.LoginAsync(loginDto);

                // User Claims
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
                    IsPersistent = Input.RememberMe,
                    ExpiresUtc = Input.RememberMe ? DateTimeOffset.UtcNow.AddDays(30) : DateTimeOffset.UtcNow.AddHours(2)
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                {
                    return LocalRedirect(ReturnUrl);
                }

                return RedirectToPage("/Index");
            }
            catch (UnauthorizedAccessException)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "An error occured during login.");
                return Page();
            }
        }

    }
}