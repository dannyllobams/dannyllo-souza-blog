using dbs.blog.Application.Commands;
using dbs.blog.Application.Queries;
using dbs.blog.Areas.Admin.Models;
using dbs.blog.DTOs;
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
        public async Task<IActionResult> NewPost()
        {
            var categoriesQueryResult = await _mediatorHandler.ProjectionQuery<CategoriesQuery, List<string>>(new CategoriesQuery());
            ViewBag.Categories = categoriesQueryResult.ValidationResult.IsValid ? categoriesQueryResult.Response : new List<string>();

            return View(new AddPostViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> NewPost(AddPostViewModel model)
        {
            if (ModelState.IsValid)
            {
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
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }

                foreach (var error in result.ValidationResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.ErrorMessage);
                }
            }           

            var categoriesQueryResult = await _mediatorHandler.ProjectionQuery<CategoriesQuery, List<string>>(new CategoriesQuery());
            ViewBag.Categories = categoriesQueryResult.ValidationResult.IsValid ? categoriesQueryResult.Response : new List<string>();

            return View(model);
        }

        [HttpGet("[area]/[controller]/[action]/{url}")]
        public async Task<IActionResult> EditPost(string url)
        {
            var postQueryResult = await _mediatorHandler.ProjectionQuery<PostQuery, PostDTO>(new PostQuery { Url = url });

            if (!postQueryResult.ValidationResult.IsValid)
            {
                foreach(var error in postQueryResult.ValidationResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.ErrorMessage);
                }
            }

            var categoriesQueryResult = await _mediatorHandler.ProjectionQuery<CategoriesQuery, List<string>>(new CategoriesQuery());
            ViewBag.Categories = categoriesQueryResult.ValidationResult.IsValid ? categoriesQueryResult.Response : new List<string>();

            return View(EditPostViewModel.FromPostDTO(postQueryResult.Response ?? new PostDTO()));
        }

        [HttpPost("[controller]/[action]/{url}")]
        public async Task<IActionResult> EditPost(string url, EditPostViewModel model)
        {
            if (ModelState.IsValid)
            {
                var command = new UpdatePostCommand
                {
                    PostId = model.Id,
                    Title = model.Title,
                    UrlSlug = model.Slug,
                    Content = model.Content,
                    Summary = model.Excerpt,
                    UrlMainImage = model.UrlMainImage,
                    SEO = new UpdatePostCommand.SEOData
                    {
                        MetaTitle = model.MetaTitle,
                        MetaDescription = model.MetaDescription
                    },
                    Categories = model.Categories,
                    Tags = model.Tags
                };

                var result = await _mediatorHandler.CallCommand<UpdatePostCommand>(command);

                if (result.IsValid)
                {
                    return RedirectToAction("EditPost", "Posts", new { url = model.Slug });
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.ErrorMessage);
                }
            }

            var categoriesQueryResult = await _mediatorHandler.ProjectionQuery<CategoriesQuery, List<string>>(new CategoriesQuery());
            ViewBag.Categories = categoriesQueryResult.ValidationResult.IsValid ? categoriesQueryResult.Response : new List<string>();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> PublishPost(PublishPostViewModel model)
        {
            var command = new PublishPostCommand
            {
                PostId = model.PostId
            };

            var result = await _mediatorHandler.CallCommand<PublishPostCommand>(command);
            if (result.IsValid)
            {
                return Ok();
            }

            return BadRequest(result.Errors.Select(e => e.ErrorMessage));
        }

        [HttpPost]
        public async Task<IActionResult> UnpublishPost(UnpublishPostViewModel model)
        {
            var command = new UnpublishPostCommand
            {
                PostId = model.PostId
            };

            var result = await _mediatorHandler.CallCommand<UnpublishPostCommand>(command);
            if (result.IsValid)
            {
                return Ok();
            }

            return BadRequest(result.Errors.Select(e => e.ErrorMessage));
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePost(Guid postId)
        {
            var command = new DeletePostCommand
            {
                PostId = postId
            };
            var result = await _mediatorHandler.CallCommand<DeletePostCommand>(command);
            if (result.IsValid)
            {
                return Ok();
            }
            return BadRequest(result.Errors.Select(e => e.ErrorMessage));
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
    }
}