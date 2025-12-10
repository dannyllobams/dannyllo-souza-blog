using FluentValidation.Results;

namespace dbs.core.Messages
{
    public abstract class CommandResult
    {
        public ValidationResult ValidationResult { get; set; }

        public CommandResult(ValidationResult validationResult)
        {
            this.ValidationResult = validationResult;
        }
    }
}
