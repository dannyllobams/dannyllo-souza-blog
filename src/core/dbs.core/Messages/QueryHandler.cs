using FluentValidation.Results;

namespace dbs.core.Messages
{
    public abstract class QueryHandler
    {
        protected readonly ValidationResult ValidationResult;

        protected QueryHandler()
        {
            ValidationResult = new ValidationResult();
        }

        protected void AddError(string errorMessage)
        {
            var error = new ValidationFailure(string.Empty, errorMessage);
            ValidationResult.Errors.Add(error);
        }

        protected QueryResult<T> Response<T>(T response)
        {
            return QueryResult<T>.Success(response, ValidationResult);
        }
    }
}
