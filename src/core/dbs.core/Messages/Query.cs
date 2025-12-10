using Cortex.Mediator.Queries;
using FluentValidation.Results;

namespace dbs.core.Messages
{
    public abstract class Query<T> : IQuery<QueryResult<T>> where T : class
    {
        public ValidationResult ValidationResult { get; set; } = new ValidationResult();
        public abstract bool EhValido();
    }
}
