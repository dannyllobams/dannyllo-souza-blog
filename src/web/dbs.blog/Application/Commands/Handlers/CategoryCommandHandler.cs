using Cortex.Mediator.Commands;
using dbs.core.Messages;
using dbs.domain.Repositories;
using FluentValidation.Results;

namespace dbs.blog.Application.Commands.Handlers
{
    public class CategoryCommandHandler : CommandHandler,
        ICommandHandler<DeleteCategoryCommand, ValidationResult>
    {
        private readonly IPostsRepository _postsRepository;

        public CategoryCommandHandler(IPostsRepository postsRepository)
        {
            _postsRepository = postsRepository;
        }

        public async Task<ValidationResult> Handle(DeleteCategoryCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
            {
                return command.ValidationResult;
            }

            var categories = await _postsRepository.GetCategoriesAsync();
            var category = categories.FirstOrDefault(c => c.Id == command.CategoryId);

            if (category == null)
            {
                AddError("Category not found.");
                return ValidationResult;
            }

            var isUsed = await _postsRepository.IsCategoryUsedAsync(category.Name);
            if (isUsed)
            {
                AddError("Cannot delete category that is being used by posts.");
                return ValidationResult;
            }

            _postsRepository.RemoveCategory(category);
            await SaveChangesAsync(_postsRepository.UnitOfWork, cancellationToken);

            return ValidationResult;
        }
    }
}

