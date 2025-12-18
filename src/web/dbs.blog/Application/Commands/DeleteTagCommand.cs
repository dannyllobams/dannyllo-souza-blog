using dbs.core.Messages;
using FluentValidation;

namespace dbs.blog.Application.Commands
{
    public class DeleteTagCommand : Command
    {
        public Guid TagId { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new DeleteTagCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class DeleteTagCommandValidator : AbstractValidator<DeleteTagCommand>
    {
        public DeleteTagCommandValidator()
        {
            RuleFor(t => t.TagId)
                .NotEqual(Guid.Empty).WithMessage("TagId must be a valid GUID.");
        }
    }
}

