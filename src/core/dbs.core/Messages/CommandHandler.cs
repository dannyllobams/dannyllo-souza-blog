using dbs.core.Data;
using FluentValidation.Results;

namespace dbs.core.Messages
{
    public abstract class CommandHandler
    {
        protected readonly ValidationResult ValidationResult;

        protected CommandHandler()
        {
            ValidationResult = new ValidationResult();
        }


        protected void AddError(string errorMessage)
        {
            var error = new ValidationFailure(string.Empty, errorMessage);
            ValidationResult.Errors.Add(error);
        }

        protected async Task<ValidationResult> SaveChangesAsync(IUnitOfWork uow,
            CancellationToken cancellationToken = default)
        {
            if(!await uow.CommitAsync())
            {
                AddError("An error occurred while trying to save data to the database.");
            }

            return ValidationResult;
        }
    }
}
