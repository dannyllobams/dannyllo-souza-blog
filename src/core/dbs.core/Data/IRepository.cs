using dbs.core.DomainObjects;

namespace dbs.core.Data
{
    public interface  IRepository 
    {
        IUnitOfWork UnitOfWork { get; }
    }

    public interface IRepository<T> : IRepository where T : Entity
    {

        Task<int> CountAsync();
        Task<T?> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync(int page, int pageSize);
        Task<Guid> AddAsync(T entity);
        void Update(T entity);
        void Remove(T entity);
    }
}
