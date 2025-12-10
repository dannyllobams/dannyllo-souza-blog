using dbs.core.Data;
using dbs.domain.Model;
using dbs.domain.Repositories;
using dbs.infra.Context;
using Microsoft.EntityFrameworkCore;

namespace dbs.infra.Repositories
{
    internal class PostsRepository : IPostsRepository
    {
        private readonly BlogContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public PostsRepository(BlogContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Post>> GetAllByCategoryAsync(Category category, int page, int pageSize)
        {
            return await _context.Posts
                .Include(post => post.Categories)
                .Include(post => post.Tags)
                .Include(post => post.Comments).ThenInclude(reply => reply.Replies)
                .AsSplitQuery()
                .Where(post => post.Categories.Any(t => t.Id == category.Id))
                .AsNoTracking()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetAllByTagAsync(Tag tag, int page, int pageSize)
        {
            return await _context.Posts
                .Include(post => post.Categories)
                .Include(post => post.Tags)
                .Include(post => post.Comments).ThenInclude(reply => reply.Replies)
                .AsSplitQuery()
                .Where(post => post.Tags.Any(t => t.Id == tag.Id))
                .AsNoTracking()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Post?> GetByIdAsync(Guid id)
        {
            return await _context.Posts
                .Include(post => post.Categories)
                .Include(post => post.Tags)
                .Include(post => post.Comments).ThenInclude(reply => reply.Replies)
                .FirstOrDefaultAsync(post => post.Id == id);
        }

        public async Task<IEnumerable<Post>> GetAllAsync(int page, int pageSize)
        {
            return await _context.Posts
                .Include(post => post.Categories)
                .Include(post => post.Tags)
                .Include(post => post.Comments).ThenInclude(reply => reply.Replies)
                .AsSplitQuery()
                .AsNoTracking()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Guid> AddAsync(Post entity)
        {
            await _context.Posts.AddAsync(entity);
            return entity.Id;
        }

        public void Update(Post entity)
        {
            _context.Posts.Update(entity);
        }

        public void Remove(Post entity)
        {
            _context.Posts.Remove(entity);
        }

        //Categories

        public async Task<Category?> GetCategoryByNameAsync(string categoryName)
        {
            return await _context.Categories
                .Include(category => category.Posts)
                .FirstOrDefaultAsync(c => c.Name == categoryName);
        }

        public async Task<bool> IsCategoryUsedAsync(string name)
        {
            return await _context.Posts
                .Include(post => post.Categories)
                .Where(post => post.Categories.Any(c => c.Name == name))
                .AnyAsync();
        }

        public void RemoveCategory(Category category)
        {
            _context.Categories.Remove(category);
        }


        //Tags
        public async Task<Tag?> GetTagByNameAsync(string tagName)
        {
            return await _context.Tags
                .Include(tag => tag.Posts)
                .FirstOrDefaultAsync(c => c.Name == tagName);
        }

        public async Task<bool> IsTagUsedAsync(string tagName)
        {
            return await _context.Posts
                .Include(post => post.Tags)
                .Where(post => post.Tags.Any(t => t.Name == tagName))
                .AnyAsync();
        }

        public void RemoveTag(Tag tag)
        {
            _context.Tags.Remove(tag);
        }
    }
}
