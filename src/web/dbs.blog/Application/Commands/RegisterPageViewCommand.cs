using dbs.core.Messages;
using FluentValidation;

namespace dbs.blog.Application.Commands
{
    public class RegisterPageViewCommand : Command
    {
        public Guid PageId { get; set; } = Guid.NewGuid();

        public override bool IsValid()
        {
            this.ValidationResult = new RegisterPageViewCommandValidator().Validate(this);
            return this.ValidationResult.IsValid;
        }
    }

    public class RegisterPageViewCommandValidator : AbstractValidator<RegisterPageViewCommand>
    {
        public RegisterPageViewCommandValidator()
        {
            RuleFor(x => x.PageId)
                .NotEqual(Guid.Empty).WithMessage("PageId is required.");
        }
    }
}
