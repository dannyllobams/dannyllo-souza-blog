using FluentValidation.Results;

namespace dbs.core.Messages
{
    public class QueryResult<T> where T : class
    {
        public T? Resultado { get; private set; }
        public ValidationResult ValidationResult { get; private set; }

        public QueryResult(T resultado, ValidationResult validationResult)
        {
            this.Resultado = resultado;
            this.ValidationResult = validationResult;
        }

        public QueryResult(ValidationResult validationResult)
        {
            this.ValidationResult = validationResult;
        }
    }
}
