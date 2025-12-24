using dbs.blog.Application.Commands;
using dbs.blog.Services;
using dbs.core.Mediator;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Caching.Memory;

namespace dbs.blog.Controllers.api
{
    [ApiController]
    public sealed class PageViewsController : ControllerBase
    {
        private ICollection<string> Errors = new List<string>();

        private const int DedupeMinutes = 30;

        private readonly IMediatorHandler _mediator;
        private readonly IMemoryCacheService _memoryCacheService;

        public PageViewsController(
            IMediatorHandler mediator,
            IMemoryCacheService memoryCacheService)
        {
            _mediator = mediator;
            _memoryCacheService = memoryCacheService;
        }

        #region CONTROLLER HELPERS
        private IActionResult CustomResponse()
        {
            if (!Errors.Any())
                return NoContent();

            return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>
            {
                { "Messages", Errors.ToArray() }
            }));
        }

        private IActionResult CustomResponse(ValidationResult validationResult)
        {
            if (validationResult.IsValid)
                return NoContent();

            foreach (var error in validationResult.Errors)
            {
                Errors.Add(error.ErrorMessage);
            }

            return CustomResponse();
        }
        #endregion

        [HttpPost]
        [Route("api/pageviews")]
        [EnableRateLimiting("pageviews")]
        public async Task<IActionResult> PostPageView(RegisterPageViewCommand command, CancellationToken cancellationToken)
        {
            if(!command.IsValid())
            {
                return CustomResponse(command.ValidationResult);
            }

            var visitorKey = BuildVisitorKey(command.PageId);
            if (_memoryCacheService.TryGetHashed(visitorKey, out _))
            {
                return CustomResponse();
            }

            var response = await _mediator.CallCommand(command, cancellationToken);

            if(response.IsValid)
            {
                _memoryCacheService.SetHashed(visitorKey, true);
            }

            return CustomResponse(response);
        }

        private string BuildVisitorKey(Guid pageId)
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var ua = Request.Headers.UserAgent.ToString();

            return $"{ip}|{ua}";
        }
    }
}
