using Cortex.Mediator.Queries;
using dbs.blog.DTOs;
using dbs.core.Messages;
using dbs.domain.Repositories;
using FluentValidation;
using FluentValidation.Results;

namespace dbs.blog.Application.Queries
{
    public class PostsQuery : Query<IEnumerable<PostListItemDTO>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public bool PublishedOnly { get; set; } = true;

        public override bool IsValid()
        {
            ValidationResult = new PostsQueryValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class PostsQueryValidator : AbstractValidator<PostsQuery>
    {
        public PostsQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0).WithMessage("Page number must be greater than zero.");
        }
    }

    public class PostsQueryHandler : QueryHandler, IQueryHandler<PostsQuery, QueryResult<IEnumerable<PostListItemDTO>>>
    {
        private readonly IPostsRepository _postsRepository;
        public PostsQueryHandler(IPostsRepository postsRepository)
        {
            _postsRepository = postsRepository;
        }

        public async Task<QueryResult<IEnumerable<PostListItemDTO>>> Handle(PostsQuery query, CancellationToken cancellationToken)
        {
            if(!query.IsValid())
            {
                return new QueryResult<IEnumerable<PostListItemDTO>>(query.ValidationResult);
            }

            var posts = query.PublishedOnly ? 
                await _postsRepository.GetAllPublishedsAsync(query.PageNumber, query.PageSize) : 
                await _postsRepository.GetAllAsync(query.PageNumber, query.PageSize);

            return Response(posts.Select(PostListItemDTO.ToPostListItemDTO));
        }
    }
}
