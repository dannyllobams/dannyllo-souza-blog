using Cortex.Mediator.Queries;
using dbs.blog.DTOs;
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
        public PostQueryHandler(IPostsRepository postsRepository)
        {
            _postsRepository = postsRepository;
        }

        public async Task<QueryResult<PostDTO>> Handle(PostQuery query, CancellationToken cancellationToken)
        {
            if(!query.IsValid())
            {
                return new QueryResult<PostDTO>(this.ValidationResult);
            }

            var post = await this._postsRepository.GetPostByUrlSlugAsync(query.Url, cancellationToken);

            if(post is null)
            {
                this.AddError("The requested post was not found.");
                return new QueryResult<PostDTO>(this.ValidationResult);
            }

            return QueryResult<PostDTO>.Success(PostDTO.ToPostDTO(post), this.ValidationResult);
        }
    }
}
