using Cortex.Mediator.Queries;
using dbs.blog.DTOs;
using dbs.core.Messages;
using dbs.domain.Repositories;
using FluentValidation.Results;

namespace dbs.blog.Application.Queries
{
    public class CategoriesListQuery : Query<IEnumerable<CategoryDTO>>
    {
        public override bool IsValid()
        {
            ValidationResult = new ValidationResult();
            return ValidationResult.IsValid;
        }
    }

    public class CategoriesListQueryHandler : QueryHandler, IQueryHandler<CategoriesListQuery, QueryResult<IEnumerable<CategoryDTO>>>
    {
        private readonly IPostsRepository _postsRepository;

        public CategoriesListQueryHandler(IPostsRepository postsRepository)
        {
            _postsRepository = postsRepository;
        }

        public async Task<QueryResult<IEnumerable<CategoryDTO>>> Handle(CategoriesListQuery query, CancellationToken cancellationToken)
        {
            if (!query.IsValid())
            {
                return new QueryResult<IEnumerable<CategoryDTO>>(query.ValidationResult);
            }

            var categories = await _postsRepository.GetCategoriesAsync();
            var categoriesDTO = new List<CategoryDTO>();

            foreach (var category in categories)
            {
                var isUsed = await _postsRepository.IsCategoryUsedAsync(category.Name);
                categoriesDTO.Add(CategoryDTO.ToCategoryDTO(category, isUsed));
            }

            return Response<IEnumerable<CategoryDTO>>(categoriesDTO);
        }
    }
}

