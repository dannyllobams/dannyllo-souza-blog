using dbs.blog.Application.Commands;
using dbs.blog.Models;
using dbs.core.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace dbs.blog.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PostsController : Controller
    {
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IWebHostEnvironment _env;

        public PostsController(IMediatorHandler mediatorHandler, IWebHostEnvironment env)
        {
            _mediatorHandler = mediatorHandler;
            _env = env;
        }

        [HttpGet]
        public IActionResult NewPost()
        {
            return View(new AddPostViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(_env.WebRootPath, "images", "posts", fileName);

            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var urlImage = $"/images/posts/{fileName}";

            return Ok(new { url = urlImage });
        }

        [HttpPost]
        public async Task<IActionResult> NewPost(AddPostViewModel model)
        {
            if (!ModelState.IsValid) return View(model);


            var command = new AddPostCommand
            {
                Title = model.Title,
                UrlSlug = model.Slug,
                Content = model.Content,
                Summary = model.Excerpt,
                UrlMainImage = model.UrlMainImage,
                SEO = new AddPostCommand.SEOData 
                { 
                    MetaTitle = model.MetaTitle, 
                    MetaDescription = model.MetaDescription 
                },
                Categories = new List<string> { model.Category },
                Tags = model.Tags
            };

            var result = await _mediatorHandler.CallCommand<AddPostCommand, AddPostCommand.Result>(command);

            if (result.ValidationResult.IsValid)
            {
                return RedirectToAction("Index"); // Redirect to list
            }

            foreach (var error in result.ValidationResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.ErrorMessage);
            }

            return View(model);
        }
    }
}