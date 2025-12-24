using Cortex.Mediator.Queries;
using dbs.blog.Basics;
using dbs.core.Messages;
using dbs.domain.Repositories;
using FluentValidation.Results;
using Microsoft.Extensions.Caching.Memory;

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
        private readonly IMemoryCache _cache;

        public CountAllPostsQueryHandler(
            IPostsRepository postsRepository,
            IMemoryCache memoryCache)
        {
            _postsRepository = postsRepository;
            _cache = memoryCache;
        }
        
        public async Task<QueryResult<int>> Handle(CountAllPostsQuery query, CancellationToken cancellationToken)
        {
            if (!query.IsValid())
            {
                return new QueryResult<int>(query.ValidationResult);
            }

            var totalPosts = await _postsRepository.CountAllAsync();
            return Response(totalPosts);
        }
    }
}

