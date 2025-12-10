using dbs.core.Messages;
using FluentValidation.Results;

namespace dbs.core.Mediator
{
    public interface IMediatorHandler
    {
        Task Publish<T>(T evento, CancellationToken cancelationToken = default) where T : Event;

        Task<ValidationResult> CallCommand<T>(T command, CancellationToken cancelationToken = default) where T : Command;

        Task<TResult> CallCommand<T, TResult>(T command, CancellationToken cancelationToken = default) 
            where T : Command<TResult> 
            where TResult : CommandResult;

        Task<QueryResult<TResult>> ProjectionQuery<T, TResult>(T query, CancellationToken cancelationToken = default)
            where T : Query<TResult> 
            where TResult : class;
    }
}