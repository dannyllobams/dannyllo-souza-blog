using Cortex.Mediator.Queries;
using dbs.core.Messages;
using dbs.domain.Repositories;

namespace dbs.blog.Application.Queries
{
    public class CategoriesQuery : Query<List<string>>
    {
        public override bool IsValid()
        {
            this.ValidationResult = new FluentValidation.Results.ValidationResult();
            return this.ValidationResult.IsValid;
        }
    }

    public class CategoriesQueryHandler : QueryHandler,
        IQueryHandler<CategoriesQuery, QueryResult<List<string>>>
    {

        private readonly IPostsRepository _postsRepository;
        public CategoriesQueryHandler(IPostsRepository postsRepository)
        {
            _postsRepository = postsRepository;
        }

        public async Task<QueryResult<List<string>>> Handle(CategoriesQuery query, CancellationToken cancellationToken)
        {
            if(!query.IsValid())
            {
                return new QueryResult<List<string>>(this.ValidationResult);
            }

            var categories = await _postsRepository.GetCategoriesAsync();

            return QueryResult<List<string>>.Success(categories.Select(cat => cat.Name).ToList(), this.ValidationResult);
        }
    }
}
