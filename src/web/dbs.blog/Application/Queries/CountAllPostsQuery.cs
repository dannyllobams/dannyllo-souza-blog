using Cortex.Mediator.Queries;
using dbs.core.Messages;
using dbs.domain.Repositories;
using FluentValidation.Results;

namespace dbs.blog.Application.Queries
{
    public class CountAllPostsQuery : Query<int>
    {
        public override bool IsValid()
        {
            ValidationResult = new ValidationResult();
            return ValidationResult.IsValid;
        }
    }

    public class CountAllPostsQueryHandler : QueryHandler, IQueryHandler<CountAllPostsQuery, QueryResult<int>>
    {
        private readonly IPostsRepository _postsRepository;
        
        public CountAllPostsQueryHandler(IPostsRepository postsRepository)
        {
            _postsRepository = postsRepository;
        }
        
        public async Task<QueryResult<int>> Handle(CountAllPostsQuery query, CancellationToken cancellationToken)
        {
            var totalPosts = await _postsRepository.CountAllAsync();
            return Response(totalPosts);
        }
    }
}

