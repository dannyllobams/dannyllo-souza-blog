using Cortex.Mediator.Commands;
using dbs.blog.Basics;
using dbs.blog.Services;
using dbs.core.Messages;
using dbs.domain.Basics.Enum;
using dbs.domain.Model;
using dbs.domain.Repositories;
using FluentValidation.Results;

namespace dbs.blog.Application.Commands.Handlers
{
    public class PostCommandHandler : CommandHandler, 
        ICommandHandler<AddPostCommand, AddPostCommand.Result>,
        ICommandHandler<UpdatePostCommand, ValidationResult>,
        ICommandHandler<DeletePostCommand, ValidationResult>,
        ICommandHandler<PublishPostCommand, ValidationResult>,
        ICommandHandler<UnpublishPostCommand, ValidationResult>
    {
        private readonly IPostsRepository _postsRepository;
        private readonly IMemoryCacheService _cache;
        public PostCommandHandler(
            IPostsRepository postsRepository,
            IMemoryCacheService cache)
        {
            _postsRepository = postsRepository;
            _cache = cache;
        }

        public async Task<AddPostCommand.Result> Handle(AddPostCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
            {
                return new AddPostCommand.Result(command.ValidationResult);
            }

            var post = await CreatePostFromCommand(command);
            var createdPost = await _postsRepository.AddAsync(post);

            await SaveChangesAsync(_postsRepository.UnitOfWork, cancellationToken);

            return new AddPostCommand.Result(createdPost, ValidationResult);
        }

        public async Task<ValidationResult> Handle(UpdatePostCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
            {
                return command.ValidationResult;
            }

            var post = await _postsRepository.GetByIdAsync(command.PostId);

            if(post == null)
            {
                AddError("Post not found.");
                return ValidationResult;
            }

            if(post.Status == PostStatus.PUBLISHED)
            {
                AddError("Published posts cannot be edited.");
                return ValidationResult;
            }

            post.Title = command.Title;
            post.UrlSlug = command.UrlSlug;
            post.UrlMainImage = command.UrlMainImage;
            post.Content = command.Content;
            post.Summary = command.Summary;
            post.SEO!.MetaTitle   = command.SEO.MetaTitle;
            post.SEO!.MetaDescription   = command.SEO.MetaDescription;
            post.Status = PostStatus.DRAFT;

            var categoriesToBeAdded = command.Categories
                .Where(c => !post.Categories.Any(pc => pc.Name == c))
                .ToList();

            var categoriesToBeRemoved = post.Categories
                .Where(pc => !command.Categories.Any(c => c == pc.Name))
                .ToList();

            var tagsToBeAdded = command.Tags
                .Where(t => !post.Tags.Any(pt => pt.Name == t))
                .ToList();

            var tagsToBeRemoved = post.Tags
                .Where(pt => !command.Tags.Any(t => t == pt.Name))
                .ToList();

            foreach (var categoryName in categoriesToBeAdded)
            {
                var category = await GetCategory(categoryName);
                post.AddCategory(category);
            }

            foreach (var category in categoriesToBeRemoved)
            {
                post.RemoveCategory(category);
            }

            foreach (var tagName in tagsToBeAdded)
            {
                var tag = await GetTag(tagName);
                post.AddTag(tag);
            }

            foreach (var tag in tagsToBeRemoved)
            {
                post.RemoveTag(tag);
            }

            _postsRepository.Update(post);
            await SaveChangesAsync(_postsRepository.UnitOfWork, cancellationToken);

            return ValidationResult;
        }

        public async Task<ValidationResult> Handle(DeletePostCommand command, CancellationToken cancellationToken)
        {
            if(!command.IsValid())
            {
                return command.ValidationResult;
            }

            var post = await _postsRepository.GetByIdAsync(command.PostId);

            if(post == null)
            {
                AddError("Post not found.");
                return ValidationResult;
            }

            if(post.Status != PostStatus.DRAFT)
            {
                AddError("Only draft posts can be deleted.");
                return ValidationResult;
            }

            _postsRepository.Remove(post);
            await SaveChangesAsync(_postsRepository.UnitOfWork, cancellationToken);

            return ValidationResult;
        }

        private async Task<Post> CreatePostFromCommand(AddPostCommand command)
        {
            var newPost = new Post(
                command.Title,
                command.UrlSlug,
                command.UrlMainImage,
                command.Content,
                command.Summary,
                command.SEO.MetaTitle,
                command.SEO.MetaDescription);

            foreach (var categoryName in command.Categories)
            {
                var category = await GetCategory(categoryName);
                newPost.AddCategory(category);
            }

            foreach (var tagName in command.Tags)
            {
                var tag = await GetTag(tagName);
                newPost.AddTag(tag);
            }

            return newPost;
        }

        public async Task<ValidationResult> Handle(PublishPostCommand command, CancellationToken cancellationToken)
        {
            if(!command.IsValid())
            {
                return command.ValidationResult;
            }

            var post = await _postsRepository.GetByIdAsync(command.PostId);

            if (post == null)
            {
                AddError("Post not found.");
                return ValidationResult;
            }

            post.Status = PostStatus.PUBLISHED;
            _postsRepository.Update(post);

            _cache.BumpVersion(CacheKeys.POSTS_NAMESPACE);

            await SaveChangesAsync(_postsRepository.UnitOfWork, cancellationToken);

            return ValidationResult;
        }

        public async Task<ValidationResult> Handle(UnpublishPostCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
            {
                return command.ValidationResult;
            }

            var post = await _postsRepository.GetByIdAsync(command.PostId);

            if (post == null)
            {
                AddError("Post not found.");
                return ValidationResult;
            }

            post.Status = PostStatus.DRAFT;
            _postsRepository.Update(post);

            _cache.BumpVersion(CacheKeys.POSTS_NAMESPACE);

            await SaveChangesAsync(_postsRepository.UnitOfWork, cancellationToken);

            return ValidationResult;
        }

        public async Task<Category> GetCategory(string categoryName)
        {
            var category = await _postsRepository.GetCategoryByNameAsync(categoryName);
            if (category == null)
            {
                category = new Category(categoryName);
                await _postsRepository.AddCategoryAsync(category);
            }

            return category;
        }

        public async Task<Tag> GetTag(string tagName)
        {
            var tag = await _postsRepository.GetTagByNameAsync(tagName);
            if (tag == null)
            {
                tag = new Tag(tagName);
                await _postsRepository.AddTagAsync(tag);
            }

            return tag;
        }
    }
}
