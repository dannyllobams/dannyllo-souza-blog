using dbs.core.Data;
using dbs.domain.Model;

namespace dbs.domain.Repositories
{
    public interface IPostsRepository : IRepository<Post>
    {
        Task<IEnumerable<Post>> GetAllPublishedsAsync(int page, int pageSize);
        Task<IEnumerable<Post>> GetAllByCategoryAsync(Category category, int page, int pageSize);
        Task<IEnumerable<Post>> GetAllByTagAsync(Tag tag, int page, int pageSize);

        //Comments
        Task<int> CountCommentsAsync();

        //Categories
        Task<Category?> GetCategoryByNameAsync(string categoryName);
        Task<bool> IsCategoryUsedAsync(string name);
        void RemoveCategory(Category category);

        //Tags
        Task<Tag?> GetTagByNameAsync(string tagName);
        Task<bool> IsTagUsedAsync(string tagName);
        void RemoveTag(Tag tag);
    }
}
