using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dbs.blog.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AllowAnonymous]
    public class AuthController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Logout()
        {
            return RedirectToAction("Index", "Home", new { area = string.Empty });
        }
    }
}
