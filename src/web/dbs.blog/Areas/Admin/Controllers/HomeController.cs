using dbs.blog.Application.Queries;
using dbs.blog.Areas.Admin.Models;
using dbs.blog.DTOs;
using dbs.core.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace dbs.blog.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private const int PAGE_SIZE = 10;
        private readonly IMediatorHandler _mediator;
        
        public HomeController(IMediatorHandler mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            var totalPostsQueryResult = await _mediator.ProjectionQuery<CountPublishedPostsQuery, int>(new CountPublishedPostsQuery());
            var totalPageViewsQueryResult = await _mediator.ProjectionQuery<PageViewsQuery, int>(new PageViewsQuery());
            var totalCommentsQueryResult = await _mediator.ProjectionQuery<CountCommentsQuery, int>(new CountCommentsQuery());
            var postsQueryResult = await _mediator.ProjectionQuery<PostsQuery, IEnumerable<PostListItemDTO>>(new PostsQuery() { PublishedOnly = false, PageNumber = 1 });
            var totalAllPostsResult = await _mediator.ProjectionQuery<CountAllPostsQuery, int>(new CountAllPostsQuery());

            var posts = postsQueryResult.ValidationResult.IsValid ? (postsQueryResult.Response?.ToList() ?? new List<PostListItemDTO>()) : new List<PostListItemDTO>();
            var totalAllPosts = totalAllPostsResult.ValidationResult.IsValid ? totalAllPostsResult.Response : 0;

            // ViewBag para o partial _PostList funcionar no dashboard
            ViewBag.CurrentPage = 1;
            ViewBag.TotalPages = (int)Math.Ceiling(totalAllPosts / (double)PAGE_SIZE);
            ViewBag.TotalItems = totalAllPosts;
            ViewBag.PageSize = PAGE_SIZE;

            var dashboardViewModel = new DashboardViewModel
            {
                TotalPosts = totalPostsQueryResult.ValidationResult.IsValid ? totalPostsQueryResult.Response : 0,
                TotalPageViews = totalPageViewsQueryResult.ValidationResult.IsValid ? totalPageViewsQueryResult.Response : 0,
                TotalComments = totalCommentsQueryResult.ValidationResult.IsValid ? totalCommentsQueryResult.Response : 0,
                Posts = posts
            };

            return View(dashboardViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> BlogPosts(int page = 1)
        {
            if (page < 1) page = 1;

            var postsQueryResult = await _mediator.ProjectionQuery<PostsQuery, IEnumerable<PostListItemDTO>>(
                new PostsQuery() { PublishedOnly = false, PageNumber = page });
            
            var totalAllPostsResult = await _mediator.ProjectionQuery<CountAllPostsQuery, int>(new CountAllPostsQuery());
            var totalAllPosts = totalAllPostsResult.ValidationResult.IsValid ? totalAllPostsResult.Response : 0;
            var totalPages = (int)Math.Ceiling(totalAllPosts / (double)PAGE_SIZE);

            var posts = postsQueryResult.ValidationResult.IsValid 
                ? (postsQueryResult.Response?.ToList() ?? new List<PostListItemDTO>()) 
                : new List<PostListItemDTO>();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalItems = totalAllPosts;
            ViewBag.PageSize = PAGE_SIZE;

            return View(posts);
        }
    }
}
