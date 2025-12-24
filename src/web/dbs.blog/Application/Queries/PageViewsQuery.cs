using Cortex.Mediator.Queries;
using dbs.core.Messages;
using dbs.domain.Repositories;

namespace dbs.blog.Application.Queries
{
    public class PageViewsQuery : Query<int>
    {
        public Guid? PostId { get; set;  }

        public override bool IsValid()
        {
            ValidationResult = new FluentValidation.Results.ValidationResult();
            return ValidationResult.IsValid;
        }
    }

    public class PageViewsQueryHandler : QueryHandler, IQueryHandler<PageViewsQuery, QueryResult<int>>
    {
        private readonly IPageViewRepository _pageViewRepository;
        public PageViewsQueryHandler(IPageViewRepository pageViewRepository)
        {
            _pageViewRepository = pageViewRepository;
        }

        public async Task<QueryResult<int>> Handle(PageViewsQuery query, CancellationToken cancellationToken)
        {
            int pageViews = 0;

            if (query.PostId.HasValue)
            {
                pageViews = await _pageViewRepository.GetTotalViews(query.PostId.Value, cancellationToken);
            }
            else
            {
                pageViews = await _pageViewRepository.GetTotalViews(cancellationToken);
            }


            return QueryResult<int>.Success(pageViews, query.ValidationResult);
        }
    }
}
