using dbs.core.Data;
using dbs.core.DomainObjects;
using dbs.infra.Context;
using Microsoft.EntityFrameworkCore;

namespace dbs.infra.Repositories
{
    public abstract class RepositoryBase<T> : IRepository<T> where T : Entity
    {
        protected readonly BlogContext _blogContext;
        public IUnitOfWork UnitOfWork => _blogContext;

        protected RepositoryBase(BlogContext blogContext)
        {
            _blogContext = blogContext;
        }

        public virtual async Task<T?> GetByIdAsync(Guid id)
        {
            return await _blogContext.Set<T>().FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(int page, int pageSize)
        {
            return await _blogContext.Set<T>()
                .AsNoTracking()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public virtual async Task<int> CountAsync()
        {
            return await _blogContext.Set<T>().CountAsync();
        }

        public virtual async Task<Guid> AddAsync(T entity)
        {
            var entry = await _blogContext.Set<T>().AddAsync(entity);
            return entry.Entity.Id;
        }

        public virtual void Update(T entity)
        {
            _blogContext.Set<T>().Update(entity);
        }

        public virtual void Remove(T entity)
        {
            _blogContext.Set<T>().Remove(entity);
        }
    }
}
