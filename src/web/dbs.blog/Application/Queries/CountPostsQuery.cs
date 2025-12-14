using Cortex.Mediator.Queries;
using dbs.core.Messages;
using FluentValidation.Results;

namespace dbs.blog.Application.Queries
{
    public class CountPostsQuery : Query<int>
    {
        public override bool EhValido()
        {
            ValidationResult = new ValidationResult();
            return ValidationResult.IsValid;
        }
    }

    public class CountPostsQueryHandler : QueryHandler, IQueryHandler<CountPostsQuery, QueryResult<int>>
    {
        private readonly dbs.domain.Repositories.IPostsRepository _postsRepository;
        public CountPostsQueryHandler(dbs.domain.Repositories.IPostsRepository postsRepository)
        {
            _postsRepository = postsRepository;
        }
        public async Task<QueryResult<int>> Handle(CountPostsQuery query, CancellationToken cancellationToken)
        {
            var totalPosts = await _postsRepository.CountAsync();
            return Response(totalPosts);
        }
    }
}
