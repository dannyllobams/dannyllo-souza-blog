using FluentValidation.Results;

namespace dbs.core.Messages
{
    public class QueryResult<T>
    {
        public T? Response { get; private set; }
        public ValidationResult ValidationResult { get; private set; }

        protected QueryResult(T response, ValidationResult validationResult) 
        {
            Response = response;
            ValidationResult = validationResult;
        }

        public QueryResult(ValidationResult validationResult)
        {
            this.ValidationResult = validationResult;
        }

        public static QueryResult<T> Success(T response, ValidationResult validationResult)
        {
            var queryResult = new QueryResult<T>(response, validationResult);
            return queryResult;
        }
    }
}
