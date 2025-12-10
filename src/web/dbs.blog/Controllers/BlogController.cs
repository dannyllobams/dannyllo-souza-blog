using Microsoft.AspNetCore.Mvc;

namespace dbs.blog.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
