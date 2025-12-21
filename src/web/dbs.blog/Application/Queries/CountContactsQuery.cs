using Cortex.Mediator.Queries;
using dbs.core.Messages;
using dbs.domain.Repositories;
using FluentValidation.Results;

namespace dbs.blog.Application.Queries
{
    public class CountContactsQuery : Query<int>
    {
        public override bool IsValid()
        {
            ValidationResult = new ValidationResult();
            return ValidationResult.IsValid;
        }
    }

    public class CountContactsQueryHandler : QueryHandler, IQueryHandler<CountContactsQuery, QueryResult<int>>
    {
        private readonly IContactRepository _contactRepository;
        
        public CountContactsQueryHandler(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }
        
        public async Task<QueryResult<int>> Handle(CountContactsQuery query, CancellationToken cancellationToken)
        {
            var totalContacts = await _contactRepository.CountAsync();
            return Response(totalContacts);
        }
    }
}

