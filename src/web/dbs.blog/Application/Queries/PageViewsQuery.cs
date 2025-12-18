using Cortex.Mediator.Queries;
using dbs.core.Messages;

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
        public Task<QueryResult<int>> Handle(PageViewsQuery query, CancellationToken cancellationToken)
        {
            int pageViews = 0;

            if (query.PostId.HasValue)
            {
                pageViews = 42;
            }
            else
            {
                pageViews = 100;
            }


            return Task.FromResult(QueryResult<int>.Success(pageViews, query.ValidationResult));
        }
    }
}
