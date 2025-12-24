using dbs.blog.Application.Queries;
using dbs.blog.DTOs;
using dbs.blog.Models;
using dbs.core.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace dbs.blog.Controllers
{
    public class BlogController : Controller
    {
        private const int PAGE_SIZE = 6;

        private readonly IMediatorHandler _mediatorHandler;
        public BlogController(IMediatorHandler mediator)
        {
            _mediatorHandler = mediator;
        }

        public async Task<IActionResult> Index()
        {
            var totalPublishedPostsResult = await _mediatorHandler.ProjectionQuery<CountPublishedPostsQuery, int>(new CountPublishedPostsQuery());
            var postsQueryResult = await _mediatorHandler.ProjectionQuery<PostsQuery, IEnumerable<PostListItemDTO>>(new PostsQuery()
            {
                PageNumber = 1,
                PageSize = PAGE_SIZE,
                PublishedOnly = true
            });

            if (postsQueryResult.ValidationResult.IsValid)
            {
                ViewBag.Posts = postsQueryResult.Response!.ToList();
            }

            var totalAllPoststotalPublishedPosts = totalPublishedPostsResult.ValidationResult.IsValid ? totalPublishedPostsResult.Response : 1;

            ViewBag.TotalPages = (int)Math.Ceiling(totalAllPoststotalPublishedPosts / (double)PAGE_SIZE);
            ViewBag.TotalPosts = totalAllPoststotalPublishedPosts;

            return View();
        }

        [HttpGet("[controller]/[action]/{url}")]
        public async Task<IActionResult> Post(string url)
        {
            var postQueryResult = await _mediatorHandler.ProjectionQuery<PostQuery, PostDTO>(new PostQuery { Url = url });

            if (!postQueryResult.ValidationResult.IsValid)
            {
                foreach (var error in postQueryResult.ValidationResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.ErrorMessage);
                }

                return View();
            }

            return View(postQueryResult.Response);
        }

        [HttpGet]
        public async Task<IActionResult> LoadPosts(int page = 1)
        {
            if (page < 1) page = 1;

            var postsQueryResult = await _mediatorHandler.ProjectionQuery<PostsQuery, IEnumerable<PostListItemDTO>>(new PostsQuery()
            {
                PageNumber = page,
                PageSize = PAGE_SIZE,
                PublishedOnly = true
            });

            if (!postsQueryResult.ValidationResult.IsValid)
            {
                return Json(new { success = false, errors = postsQueryResult.ValidationResult.Errors.Select(e => e.ErrorMessage) });
            }

            var posts = postsQueryResult.Response?.ToList() ?? new List<PostListItemDTO>();

            return Json(new { success = true, posts });
        }
    }
}
