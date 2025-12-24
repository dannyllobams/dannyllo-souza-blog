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
        private readonly IMemoryCache _cache;
        private readonly IMemorySeededHashService _memorySeededHashService;

        public PageViewsController(
            IMediatorHandler mediator,
            IMemoryCache cache,
            IMemorySeededHashService memorySeededHashService)
        {
            _mediator = mediator;
            _cache = cache;
            _memorySeededHashService = memorySeededHashService;
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
            if (_cache.TryGetValue(visitorKey, out _))
            {
                return CustomResponse();
            }

            var response = await _mediator.CallCommand(command, cancellationToken);

            if(response.IsValid)
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(DedupeMinutes)
                };

                _cache.Set(visitorKey, true, cacheEntryOptions);
            }

            return CustomResponse(response);
        }

        private string BuildVisitorKey(Guid pageId)
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var ua = Request.Headers.UserAgent.ToString();

            var visitorHash = $"{ip}|{ua}";

            return _memorySeededHashService.ComputeHash($"pv:{pageId}:{visitorHash}");
        }
    }
}
