using dbs.core.Messages;
using FluentValidation;

namespace dbs.blog.Application.Commands
{
    public class PublishPostCommand : Command
    {
        public Guid PostId { get; set; }

        public override bool IsValid()
        {
            this.ValidationResult = new PublishPostCommandValidator().Validate(this);
            return this.ValidationResult.IsValid;
        }
    }

    public class PublishPostCommandValidator : AbstractValidator<PublishPostCommand>
    {
        public PublishPostCommandValidator()
        {
            RuleFor(post => post.PostId)
                .NotEqual(post => Guid.Empty).WithMessage("PostId must be a valid GUID.");
        }
    }
}
