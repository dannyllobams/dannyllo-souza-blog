using dbs.core.Messages;
using FluentValidation;

namespace dbs.blog.Application.Commands
{
    public class MarkContactAsReceivedCommand : Command
    {
        public Guid ContactId { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new MarkContactAsReceivedCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class MarkContactAsReceivedCommandValidator : AbstractValidator<MarkContactAsReceivedCommand>
    {
        public MarkContactAsReceivedCommandValidator()
        {
            RuleFor(c => c.ContactId)
                .NotEqual(Guid.Empty).WithMessage("ContactId must be a valid GUID.");
        }
    }
}

