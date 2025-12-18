using dbs.domain.Basics.Enum;
using dbs.domain.Model;
using dbs.domain.Repositories;
using dbs.infra.Context;
using Microsoft.EntityFrameworkCore;

namespace dbs.infra.Repositories
{
    public class PostsRepository : RepositoryBase<Post>, IPostsRepository
    {
        public PostsRepository(BlogContext context) : base(context)
        {
        }

        public override Task<int> CountAsync()
        {
            return _blogContext.Posts
                .Where(post => post.Status == PostStatus.PUBLISHED)
                .CountAsync();
        }

        public async Task<int> CountAllAsync()
        {
            return await _blogContext.Posts.CountAsync();
        }

        public async Task<Post?> GetPostByUrlSlugAsync(string urlSlug, CancellationToken cancellationToken = default)
        {
            var post = await _blogContext.Posts
                .Include(p => p.Categories)
                .Include(p => p.Tags)
                .Include(p => p.Comments).ThenInclude(c => c.Replies)
                .AsSplitQuery()
                .FirstOrDefaultAsync(p => p.UrlSlug == urlSlug, cancellationToken);

            return post;
        }

        public async Task<IEnumerable<Post>> GetAllPublishedsAsync(int page, int pageSize)
        {
            return await _blogContext.Posts
                .Include(post => post.Categories)
                .Include(post => post.Tags)
                .Include(post => post.Comments).ThenInclude(c => c.Replies)
                .AsSplitQuery()
                .Where(post => post.Status == PostStatus.PUBLISHED)
                .OrderByDescending(post => post.CreatedAt)
                .AsNoTracking()
                .OrderByDescending(post => post.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetAllByCategoryAsync(Category category, int page, int pageSize)
        {
            return await _blogContext.Posts
                .Include(post => post.Categories)
                .Include(post => post.Tags)
                .Include(post => post.Comments).ThenInclude(c => c.Replies)
                .AsSplitQuery()
                .Where(post => post.Categories.Any(t => t.Id == category.Id))
                .AsNoTracking()
                .OrderByDescending(post => post.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetAllByTagAsync(Tag tag, int page, int pageSize)
        {
            return await _blogContext.Posts
                .Include(post => post.Categories)
                .Include(post => post.Tags)
                .Include(post => post.Comments).ThenInclude(c => c.Replies)
                .AsSplitQuery()
                .Where(post => post.Tags.Any(t => t.Id == tag.Id))                
                .AsNoTracking()
                .OrderByDescending(post => post.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public override async Task<Post?> GetByIdAsync(Guid id)
        {
            return await _blogContext.Posts
                .Include(post => post.Categories)
                .Include(post => post.Tags)
                .Include(post => post.Comments).ThenInclude(c => c.Replies)
                .FirstOrDefaultAsync(post => post.Id == id);
        }

        public override async Task<IEnumerable<Post>> GetAllAsync(int page, int pageSize)
        {
            return await _blogContext.Posts
                .Include(post => post.Categories)
                .Include(post => post.Tags)
                .Include(post => post.Comments).ThenInclude(c => c.Replies)
                .AsSplitQuery()
                .AsNoTracking()
                .OrderByDescending(post => post.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        //Comments
        public async Task<int> CountCommentsAsync()
        {
            return await _blogContext.Comments.CountAsync();
        }

        //Categories
        public async Task AddCategoryAsync(Category category)
        {
            await _blogContext.Categories.AddAsync(category);
        }


        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await _blogContext.Categories
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Category?> GetCategoryByNameAsync(string categoryName)
        {
            return await _blogContext.Categories
                .Include(category => category.Posts)
                .FirstOrDefaultAsync(c => c.Name == categoryName);
        }

        public async Task<bool> IsCategoryUsedAsync(string name)
        {
            return await _blogContext.Posts
                .Include(post => post.Categories)
                .Where(post => post.Categories.Any(c => c.Name == name))
                .AnyAsync();
        }

        public void RemoveCategory(Category category)
        {
            _blogContext.Categories.Remove(category);
        }


        //Tags
        public async Task AddTagAsync(Tag tag)
        {
            await _blogContext.Tags.AddAsync(tag);
        }

        public async Task<IEnumerable<Tag>> GetTagsAsync()
        {
            return await _blogContext.Tags
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Tag?> GetTagByNameAsync(string tagName)
        {
            return await _blogContext.Tags
                .Include(tag => tag.Posts)
                .FirstOrDefaultAsync(c => c.Name == tagName);
        }

        public async Task<bool> IsTagUsedAsync(string tagName)
        {
            return await _blogContext.Posts
                .Include(post => post.Tags)
                .Where(post => post.Tags.Any(t => t.Name == tagName))
                .AnyAsync();
        }

        public void RemoveTag(Tag tag)
        {
            _blogContext.Tags.Remove(tag);
        }
    }
}
