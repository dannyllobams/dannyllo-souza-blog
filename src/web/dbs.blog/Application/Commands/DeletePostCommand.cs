using dbs.core.Messages;
using FluentValidation;

namespace dbs.blog.Application.Commands
{
    public class DeletePostCommand : Command
    {
        public Guid PostId { get; set; }

        public override bool IsValid()
        {
            this.ValidationResult = new DeletePostCommandValidator().Validate(this);
            return this.ValidationResult.IsValid;
        }
    }

    public class DeletePostCommandValidator : AbstractValidator<DeletePostCommand>
    {
        public DeletePostCommandValidator()
        {
            RuleFor(post => post.PostId)
                .NotEqual(post => Guid.Empty).WithMessage("PostId must be a valid GUID.");
        }
    }
}
