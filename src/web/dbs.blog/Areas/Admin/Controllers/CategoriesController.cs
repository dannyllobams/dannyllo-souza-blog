using dbs.blog.Application.Commands;
using dbs.blog.Application.Queries;
using dbs.blog.DTOs;
using dbs.core.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace dbs.blog.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly IMediatorHandler _mediatorHandler;

        public CategoriesController(IMediatorHandler mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categoriesQueryResult = await _mediatorHandler.ProjectionQuery<CategoriesListQuery, IEnumerable<CategoryDTO>>(new CategoriesListQuery());

            var categories = categoriesQueryResult.ValidationResult.IsValid
                ? (categoriesQueryResult.Response?.ToList() ?? new List<CategoryDTO>())
                : new List<CategoryDTO>();

            return View(categories);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCategory(Guid categoryId)
        {
            var command = new DeleteCategoryCommand
            {
                CategoryId = categoryId
            };

            var result = await _mediatorHandler.CallCommand<DeleteCategoryCommand>(command);
            
            if (result.IsValid)
            {
                return Ok();
            }

            return BadRequest(result.Errors.Select(e => e.ErrorMessage));
        }
    }
}

