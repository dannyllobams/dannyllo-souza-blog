using Cortex.Mediator.Queries;
using dbs.blog.Basics;
using dbs.blog.Services;
using dbs.core.Messages;
using dbs.domain.Repositories;
using FluentValidation.Results;
using Microsoft.Extensions.Caching.Memory;

namespace dbs.blog.Application.Queries
{
    public class CountPublishedPostsQuery : Query<int>
    {
        public override bool IsValid()
        {
            ValidationResult = new ValidationResult();
            return ValidationResult.IsValid;
        }
    }

    public class CountPublishedPostsQueryHandler : QueryHandler, IQueryHandler<CountPublishedPostsQuery, QueryResult<int>>
    {
        private readonly IPostsRepository _postsRepository;
        private readonly IMemoryCacheService _cache;
        public CountPublishedPostsQueryHandler(
            IPostsRepository postsRepository,
            IMemoryCacheService cache)
        {
            _postsRepository = postsRepository;
            _cache = cache;
        }
        public async Task<QueryResult<int>> Handle(CountPublishedPostsQuery query, CancellationToken cancellationToken)
        {
            if(!query.IsValid())
            {
                return new QueryResult<int>(query.ValidationResult);
            }

            var cacheKey = _cache.BuildVersionedKey(CacheKeys.POSTS_NAMESPACE, CacheKeys.BLOG_POSTS_COUNT);

            if(_cache.TryGet<int>(cacheKey, out int cachedValue))
            {
                return Response(cachedValue);
            }

            var totalPosts = await _postsRepository.CountAsync();
            _cache.Set(cacheKey, totalPosts);

            return Response(totalPosts);
        }
    }
}
