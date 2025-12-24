using Cortex.Mediator.Queries;
using dbs.blog.Basics;
using dbs.blog.DTOs;
using dbs.blog.Services;
using dbs.core.Messages;
using dbs.domain.Repositories;
using FluentValidation;

namespace dbs.blog.Application.Queries
{
    public class PostQuery : Query<PostDTO>
    {
        public string Url { get; set; } = string.Empty;

        public override bool IsValid()
        {
            this.ValidationResult = new PostQueryValidator().Validate(this);
            return this.ValidationResult.IsValid;
        }
    }

    public class PostQueryValidator : AbstractValidator<PostQuery>
    {
        public PostQueryValidator()
        {
            RuleFor(x => x.Url)
                .NotEmpty().WithMessage("The URL slug must be provided.");
        }
    }

    public class PostQueryHandler : QueryHandler, IQueryHandler<PostQuery, QueryResult<PostDTO>>
    {
        private readonly IPostsRepository _postsRepository;
        private readonly IMemoryCacheService _cache;
        public PostQueryHandler(
            IPostsRepository postsRepository,
            IMemoryCacheService cache)
        {
            _postsRepository = postsRepository;
            _cache = cache;
        }

        public async Task<QueryResult<PostDTO>> Handle(PostQuery query, CancellationToken cancellationToken)
        {
            if(!query.IsValid())
            {
                return new QueryResult<PostDTO>(this.ValidationResult);
            }

            var cacheKey = _cache.BuildVersionedKey(CacheKeys.POSTS_NAMESPACE, CacheKeys.BLOG_POST(query.Url));

            if (_cache.TryGet(cacheKey, out PostDTO? cachedPost))
            {
                return QueryResult<PostDTO>.Success(cachedPost!, this.ValidationResult);
            }

            var post = await this._postsRepository.GetPostByUrlSlugAsync(query.Url, cancellationToken);

            if(post is null)
            {
                this.AddError("The requested post was not found.");
                return new QueryResult<PostDTO>(this.ValidationResult);
            }

            var response = PostDTO.ToPostDTO(post);
            _cache.Set(cacheKey, response);

            return QueryResult<PostDTO>.Success(response, this.ValidationResult);
        }
    }
}
