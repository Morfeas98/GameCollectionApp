using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using GameCollection.Application.Services;

namespace GameCollection.API.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly IUserService _userService;

        public RegisterModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        
        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Please choose a username")]
            [MinLength(3, ErrorMessage = "Username must be at least 3 characters")]
            [MaxLength(50, ErrorMessage = "Username cannot exceed 50 characters")]
            [Display(Name = "Username")]
            public string Username { get; set; }

            [Required(ErrorMessage = "Please enter your email")]
            [EmailAddress(ErrorMessage = "Please eneter a valid email address")]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Please choose a password")]
            [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm Password")]
            [Compare("Password", ErrorMessage = "Passwords do not match")]
            public string ConfirmPassword { get; set; }
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
                // Registration with IUserService
                var registerDto = new Application.DTOs.RegisterDto()
                {
                    Username = Input.Username,
                    Email = Input.Email,
                    Password = Input.Password
                };

                var authResponse = await _userService.RegisterAsync(registerDto);

                // Auto-Login after Register
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, authResponse.UserId.ToString()),
                    new Claim(ClaimTypes.Name, authResponse.Username),
                    new Claim(ClaimTypes.Email, authResponse.Email),
                    new Claim("Token", authResponse.Token)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                {
                    return LocalRedirect(ReturnUrl);
                }

                return RedirectToPage("/Index");
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "An error occured during registration.");
                return Page();
            }
        }
    }
}
