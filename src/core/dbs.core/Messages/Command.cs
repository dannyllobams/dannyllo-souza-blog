using Cortex.Mediator.Commands;
using FluentValidation.Results;
using System.Text.Json.Serialization;

namespace dbs.core.Messages
{
    public abstract class Command : ICommand<ValidationResult>
    {
        public DateTime Timestamp { get; private set; }

        [JsonIgnore]
        public ValidationResult ValidationResult { get; set; } = new ValidationResult();

        protected Command()
        {
            Timestamp = DateTime.Now;
        }

        public abstract bool EhValido();
    }

    public abstract class Command<TResult> : Command, ICommand<TResult> where TResult : CommandResult
    {
        protected Command()
        {
            
        }
    }
}
