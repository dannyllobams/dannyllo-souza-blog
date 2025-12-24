using Cortex.Mediator.Commands;
using dbs.core.Messages;
using dbs.domain.Repositories;
using FluentValidation.Results;

namespace dbs.blog.Application.Commands.Handlers
{
    public class PageViewCommandHandler : CommandHandler, ICommandHandler<RegisterPageViewCommand, ValidationResult>
    {
        private readonly IPageViewRepository _pageViewRepository;
        public PageViewCommandHandler(IPageViewRepository pageViewRepository)
        {
            _pageViewRepository = pageViewRepository;
        }

        public async Task<ValidationResult> Handle(RegisterPageViewCommand command, CancellationToken cancellationToken)
        {
            if(!command.IsValid())
            {
                return command.ValidationResult;
            }

            await _pageViewRepository.IncrementPageView(command.PageId);

            return this.ValidationResult;
        }
    }
}
