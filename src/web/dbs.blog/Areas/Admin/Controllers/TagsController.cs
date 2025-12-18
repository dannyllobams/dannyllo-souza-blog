using dbs.blog.Application.Commands;
using dbs.blog.Application.Queries;
using dbs.blog.DTOs;
using dbs.core.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace dbs.blog.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TagsController : Controller
    {
        private readonly IMediatorHandler _mediatorHandler;

        public TagsController(IMediatorHandler mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var tagsQueryResult = await _mediatorHandler.ProjectionQuery<TagsListQuery, IEnumerable<TagDTO>>(new TagsListQuery());

            var tags = tagsQueryResult.ValidationResult.IsValid
                ? (tagsQueryResult.Response?.ToList() ?? new List<TagDTO>())
                : new List<TagDTO>();

            return View(tags);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTag(Guid tagId)
        {
            var command = new DeleteTagCommand
            {
                TagId = tagId
            };

            var result = await _mediatorHandler.CallCommand<DeleteTagCommand>(command);
            
            if (result.IsValid)
            {
                return Ok();
            }

            return BadRequest(result.Errors.Select(e => e.ErrorMessage));
        }
    }
}

