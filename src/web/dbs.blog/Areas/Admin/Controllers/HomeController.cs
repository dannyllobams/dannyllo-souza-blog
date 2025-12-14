using dbs.blog.Application.Queries;
using dbs.blog.Areas.Admin.Models;
using dbs.blog.DTOs;
using dbs.core.Mediator;
using dbs.core.Messages;
using Microsoft.AspNetCore.Mvc;

namespace dbs.blog.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly IMediatorHandler _mediator;
        public HomeController(IMediatorHandler mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            var totalPostsQueryResult = await _mediator.ProjectionQuery<CountPostsQuery, int>(new CountPostsQuery());
            var totalPageViewsQueryResult = await _mediator.ProjectionQuery<PageViewsQuery, int>(new PageViewsQuery());
            var totalCommentsQueryResult = await _mediator.ProjectionQuery<CountCommentsQuery, int>(new CountCommentsQuery());
            var postsQueryResult = await _mediator.ProjectionQuery<PostsQuery, IEnumerable<PostListItemDTO>>(new PostsQuery());

            var dashboardViewModel = new DashboardViewModel
            {
                TotalPosts = totalPostsQueryResult.ValidationResult.IsValid ? totalPostsQueryResult.Response : 0,
                TotalPageViews = totalPageViewsQueryResult.ValidationResult.IsValid ? totalPageViewsQueryResult.Response : 0,
                TotalComments = totalCommentsQueryResult.ValidationResult.IsValid ? totalCommentsQueryResult.Response : 0,
                Posts = postsQueryResult.ValidationResult.IsValid ? (postsQueryResult.Response?.Take(5).ToList() ?? new List<PostListItemDTO>()) : new()
            };


            return View(dashboardViewModel);
        }
    }
}
