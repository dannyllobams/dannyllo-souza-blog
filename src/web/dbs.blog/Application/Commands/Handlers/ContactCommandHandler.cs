using Cortex.Mediator.Commands;
using dbs.blog.Application.Commands;
using dbs.core.Messages;
using dbs.domain.Model;
using dbs.domain.Repositories;
using FluentValidation.Results;

namespace dbs.blog.Application.Commands.Handlers
{
    public class ContactCommandHandler : CommandHandler,
        ICommandHandler<SubmitContactCommand, ValidationResult>,
        ICommandHandler<MarkContactAsReceivedCommand, ValidationResult>
    {
        private readonly IContactRepository _contactRepository;
        public ContactCommandHandler(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public async Task<ValidationResult> Handle(SubmitContactCommand command, CancellationToken cancellationToken)
        {
            if(!command.IsValid())
            {
                return command.ValidationResult;
            }

            var contact = new Contact(command.Name, command.Email, command.Message);

            await _contactRepository.AddAsync(contact);
            await SaveChangesAsync(_contactRepository.UnitOfWork);

            return ValidationResult;
        }

        public async Task<ValidationResult> Handle(MarkContactAsReceivedCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
            {
                return command.ValidationResult;
            }

            var contact = await _contactRepository.GetByIdAsync(command.ContactId);

            if (contact == null)
            {
                AddError("Contact not found.");
                return ValidationResult;
            }

            contact.MarkAsReceived();
            _contactRepository.Update(contact);
            await SaveChangesAsync(_contactRepository.UnitOfWork, cancellationToken);

            return ValidationResult;
        }
    }
}
