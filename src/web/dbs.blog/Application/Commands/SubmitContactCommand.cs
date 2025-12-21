using dbs.core.DomainObjects;
using dbs.core.Messages;
using FluentValidation;

namespace dbs.blog.Application.Commands
{
    public class SubmitContactCommand : Command
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;

        public SubmitContactCommand()
        {
            
        }

        public SubmitContactCommand(string name, string email, string message)
        {
            this.Name = name;
            this.Email = email;
            this.Message = message;
        }

        public override bool IsValid()
        {
            this.ValidationResult = new SubmitContactCommandValidator().Validate(this);
            return this.ValidationResult.IsValid;
        }
    }

    public class SubmitContactCommandValidator : AbstractValidator<SubmitContactCommand>
    {
        public SubmitContactCommandValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(200).WithMessage("Name cannot exceed 200 characters.");

            RuleFor(c => c.Email)
                .NotEmpty().WithMessage("Email is required.")
                .Must(HasValidEmail).WithMessage("O e-mail informado não é válido.");

            RuleFor(c => c.Message)
                .NotEmpty().WithMessage("Message is required.")
                .MaximumLength(1000).WithMessage("Message cannot exceed 1000 characters.");
        }

        protected static bool HasValidEmail(string email)
        {
            return Email.Validate(email);
        }
    }
}
