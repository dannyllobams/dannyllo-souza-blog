using Microsoft.AspNetCore.Mvc;

namespace dbs.blog.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PostsController : Controller
    {
        public IActionResult NewPost()
        {
            return View();
        }
    }
}