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

        public async Task<IEnumerable<Post>> GetAllPublishedsAsync(int page, int pageSize)
        {
            return await _blogContext.Posts
                .Include(post => post.Categories)
                .Include(post => post.Tags)
                .Include(post => post.Comments).ThenInclude(reply => reply.Replies)
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
                .Include(post => post.Comments).ThenInclude(reply => reply.Replies)
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
                .Include(post => post.Comments).ThenInclude(reply => reply.Replies)
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
                .Include(post => post.Comments).ThenInclude(reply => reply.Replies)
                .FirstOrDefaultAsync(post => post.Id == id);
        }

        public override async Task<IEnumerable<Post>> GetAllAsync(int page, int pageSize)
        {
            return await _blogContext.Posts
                .Include(post => post.Categories)
                .Include(post => post.Tags)
                .Include(post => post.Comments).ThenInclude(reply => reply.Replies)
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
            return await _blogContext.Categories.CountAsync();
        }

        //Categories
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
