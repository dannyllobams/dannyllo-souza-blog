using dbs.core.Messages;
using FluentValidation;

namespace dbs.blog.Application.Commands
{
    public class DeleteCategoryCommand : Command
    {
        public Guid CategoryId { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new DeleteCategoryCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
    {
        public DeleteCategoryCommandValidator()
        {
            RuleFor(c => c.CategoryId)
                .NotEqual(Guid.Empty).WithMessage("CategoryId must be a valid GUID.");
        }
    }
}

