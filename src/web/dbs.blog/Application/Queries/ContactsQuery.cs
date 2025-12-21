using Cortex.Mediator.Queries;
using dbs.blog.DTOs;
using dbs.core.Messages;
using dbs.domain.Repositories;
using FluentValidation;
using FluentValidation.Results;

namespace dbs.blog.Application.Queries
{
    public class ContactsQuery : Query<IEnumerable<ContactDTO>>
    {
        public int PageNumber { get; set; } = 1;

        public override bool IsValid()
        {
            ValidationResult = new ContactsQueryValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class ContactsQueryValidator : AbstractValidator<ContactsQuery>
    {
        public ContactsQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0).WithMessage("Page number must be greater than zero.");
        }
    }

    public class ContactsQueryHandler : QueryHandler, IQueryHandler<ContactsQuery, QueryResult<IEnumerable<ContactDTO>>>
    {
        private const int PAGE_SIZE = 10;

        private readonly IContactRepository _contactRepository;
        
        public ContactsQueryHandler(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public async Task<QueryResult<IEnumerable<ContactDTO>>> Handle(ContactsQuery query, CancellationToken cancellationToken)
        {
            if (!query.IsValid())
            {
                return new QueryResult<IEnumerable<ContactDTO>>(query.ValidationResult);
            }

            var contacts = await _contactRepository.GetAllAsync(query.PageNumber, PAGE_SIZE);

            return Response<IEnumerable<ContactDTO>>(contacts.Select(ContactDTO.ToContactDTO));
        }
    }
}

