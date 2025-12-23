using dbs.blog.Basics;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace dbs.blog.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private readonly AdministratorSettings _administratorSettings;
        public AuthController(IOptions<AdministratorSettings> administratorSettings)
        {
            _administratorSettings = administratorSettings.Value;
        }

        public IActionResult Login()
        {
            if(HttpContext.User.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password, string? returnUrl)
        {
            if (email != _administratorSettings.Email || password != _administratorSettings.Password)
            {
                ModelState.AddModelError("", "Invalid username or password.");
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, email),
                new Claim(ClaimTypes.Name, email),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
                });

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                return LocalRedirect(returnUrl);

            return Redirect("/Admin");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home", new { area = string.Empty });
        }
    }
}
