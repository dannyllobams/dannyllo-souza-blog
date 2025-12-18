using Cortex.Mediator.Queries;
using dbs.blog.DTOs;
using dbs.core.Messages;
using dbs.domain.Repositories;
using FluentValidation.Results;

namespace dbs.blog.Application.Queries
{
    public class TagsListQuery : Query<IEnumerable<TagDTO>>
    {
        public override bool IsValid()
        {
            ValidationResult = new ValidationResult();
            return ValidationResult.IsValid;
        }
    }

    public class TagsListQueryHandler : QueryHandler, IQueryHandler<TagsListQuery, QueryResult<IEnumerable<TagDTO>>>
    {
        private readonly IPostsRepository _postsRepository;

        public TagsListQueryHandler(IPostsRepository postsRepository)
        {
            _postsRepository = postsRepository;
        }

        public async Task<QueryResult<IEnumerable<TagDTO>>> Handle(TagsListQuery query, CancellationToken cancellationToken)
        {
            if (!query.IsValid())
            {
                return new QueryResult<IEnumerable<TagDTO>>(query.ValidationResult);
            }

            var tags = await _postsRepository.GetTagsAsync();
            var tagsDTO = new List<TagDTO>();

            foreach (var tag in tags)
            {
                var isUsed = await _postsRepository.IsTagUsedAsync(tag.Name);
                tagsDTO.Add(TagDTO.ToTagDTO(tag, isUsed));
            }

            return Response<IEnumerable<TagDTO>>(tagsDTO);
        }
    }
}

