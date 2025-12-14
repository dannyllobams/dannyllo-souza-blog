using dbs.core.DomainObjects;

namespace dbs.core.Data
{
    public interface IRepository<T> where T : Entity
    {
        IUnitOfWork UnitOfWork { get; }

        Task<int> CountAsync();
        Task<T?> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync(int page, int pageSize);
        Task<Guid> AddAsync(T entity);
        void Update(T entity);
        void Remove(T entity);
    }
}
