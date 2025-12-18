using dbs.core.Messages;
using FluentValidation;

namespace dbs.blog.Application.Commands
{
    public class UnpublishPostCommand : Command
    {
        public Guid PostId { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new UnpublishPostCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class UnpublishPostCommandValidator : AbstractValidator<UnpublishPostCommand>
    {
        public UnpublishPostCommandValidator()
        {
            RuleFor(post => post.PostId)
                .NotEqual(post => Guid.Empty).WithMessage("PostId must be a valid GUID.");
        }
    }
}
