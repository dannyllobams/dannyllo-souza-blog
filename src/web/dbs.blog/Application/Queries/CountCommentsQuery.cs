using Cortex.Mediator.Queries;
using dbs.core.Messages;
using dbs.domain.Repositories;
using FluentValidation.Results;

namespace dbs.blog.Application.Queries
{
    public class CountCommentsQuery : Query<int>
    {
        public override bool EhValido()
        {
            ValidationResult = new ValidationResult();
            return ValidationResult.IsValid;
        }
    }

    public class CountCommentsQueryHandler : QueryHandler, IQueryHandler<CountCommentsQuery, QueryResult<int>>
    {
        private readonly IPostsRepository _postsRepository;

        public CountCommentsQueryHandler(IPostsRepository postsRepository)
        {
            _postsRepository = postsRepository;
        }

        public async Task<QueryResult<int>> Handle(CountCommentsQuery query, CancellationToken cancellationToken)
        {
            var totalMessages = await _postsRepository.CountCommentsAsync();
            return Response(totalMessages);
        }
    }
}
