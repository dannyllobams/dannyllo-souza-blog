using Cortex.Mediator.Queries;
using dbs.blog.DTOs;
using dbs.core.Messages;
using dbs.domain.Repositories;
using FluentValidation;
using FluentValidation.Results;

namespace dbs.blog.Application.Queries
{
    public class ContactQuery : Query<ContactDTO>
    {
        public Guid ContactId { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new ContactQueryValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class ContactQueryValidator : AbstractValidator<ContactQuery>
    {
        public ContactQueryValidator()
        {
            RuleFor(x => x.ContactId)
                .NotEqual(Guid.Empty).WithMessage("ContactId must be a valid GUID.");
        }
    }

    public class ContactQueryHandler : QueryHandler, IQueryHandler<ContactQuery, QueryResult<ContactDTO>>
    {
        private readonly IContactRepository _contactRepository;
        
        public ContactQueryHandler(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public async Task<QueryResult<ContactDTO>> Handle(ContactQuery query, CancellationToken cancellationToken)
        {
            if (!query.IsValid())
            {
                return new QueryResult<ContactDTO>(query.ValidationResult);
            }

            var contact = await _contactRepository.GetByIdAsync(query.ContactId);

            if (contact == null)
            {
                AddError("Contact not found.");
                return new QueryResult<ContactDTO>(ValidationResult);
            }

            return Response<ContactDTO>(ContactDTO.ToContactDTO(contact));
        }
    }
}

