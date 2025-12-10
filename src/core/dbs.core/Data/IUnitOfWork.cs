namespace dbs.core.Data
{
    public interface IUnitOfWork
    {
        Task<bool> CommitAsync();
    }
}
