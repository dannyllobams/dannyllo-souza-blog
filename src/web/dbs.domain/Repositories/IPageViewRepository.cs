using dbs.core.Data;

namespace dbs.domain.Repositories
{
    public interface IPageViewRepository : IRepository
    {
        Task<int> GetTotalViews(CancellationToken cancellationToken = default);
        Task<int> GetTotalViews(Guid pageId, CancellationToken cancellationToken = default);
        Task IncrementPageView(Guid pageId, CancellationToken cancellationToken = default);
    }
}
