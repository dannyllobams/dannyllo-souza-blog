using dbs.blog.Application.Commands;
using dbs.blog.Application.Queries;
using dbs.blog.Areas.Admin.Models;
using dbs.blog.Basics;
using dbs.blog.DTOs;
using dbs.blog.Models;
using dbs.blog.Services;
using dbs.core.Mediator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace dbs.blog.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PostsController : Controller
    {
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IMediaStorageService _mediaStorageService;

        private readonly AppSettings _appSettings;

        public PostsController(
            IMediatorHandler mediatorHandler,
            IMediaStorageService mediaStorageService,
            IOptions<AppSettings> appSettings)
        {
            _mediatorHandler = mediatorHandler;
            _mediaStorageService = mediaStorageService;
            _appSettings = appSettings.Value;
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

        [HttpPost("[area]/[controller]/[action]/{url}")]
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
            using (var stream = file.OpenReadStream())
            {
                var uniqueFileName = await _mediaStorageService.UploadFile(file.FileName, stream);
                var urlImage = Path.Join(_appSettings.AssetsUrl, uniqueFileName);

                return Ok(new { url = urlImage });
            }
        }
    }
}