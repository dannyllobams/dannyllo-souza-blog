using Cortex.Mediator.Commands;
using dbs.core.Messages;
using dbs.domain.Repositories;
using FluentValidation.Results;

namespace dbs.blog.Application.Commands.Handlers
{
    public class TagCommandHandler : CommandHandler,
        ICommandHandler<DeleteTagCommand, ValidationResult>
    {
        private readonly IPostsRepository _postsRepository;

        public TagCommandHandler(IPostsRepository postsRepository)
        {
            _postsRepository = postsRepository;
        }

        public async Task<ValidationResult> Handle(DeleteTagCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
            {
                return command.ValidationResult;
            }

            var tags = await _postsRepository.GetTagsAsync();
            var tag = tags.FirstOrDefault(t => t.Id == command.TagId);

            if (tag == null)
            {
                AddError("Tag not found.");
                return ValidationResult;
            }

            var isUsed = await _postsRepository.IsTagUsedAsync(tag.Name);
            if (isUsed)
            {
                AddError("Cannot delete tag that is being used by posts.");
                return ValidationResult;
            }

            _postsRepository.RemoveTag(tag);
            await SaveChangesAsync(_postsRepository.UnitOfWork, cancellationToken);

            return ValidationResult;
        }
    }
}

