using Cortex.Mediator;
using dbs.core.Messages;
using FluentValidation.Results;

namespace dbs.core.Mediator
{
    public class MediatorHandler : IMediatorHandler
    {
        private readonly IMediator _mediator;

        public MediatorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<ValidationResult> CallCommand<T>(T command, CancellationToken cancelationToken = default) where T : Command
        {
            return await _mediator.SendCommandAsync<T, ValidationResult>(command, cancelationToken);
        }

        public async Task<TResult> CallCommand<T, TResult>(T command, CancellationToken cancelationToken = default)
            where T : Command<TResult>
            where TResult : CommandResult
        {
            return await _mediator.SendCommandAsync<T, TResult>(command, cancelationToken);
        }

        public async Task<QueryResult<TResult>> ProjectionQuery<T, TResult>(T query, CancellationToken cancelationToken = default)
            where T : Query<TResult>
            where TResult : class
        {
            return await _mediator.SendQueryAsync<T, QueryResult<TResult>>(query, cancelationToken);
        }

        public async Task Publish<T>(T evento, CancellationToken cancelationToken = default) where T : Event
        {
            await _mediator.PublishAsync<T>(evento, cancelationToken);
        }
    }
}
