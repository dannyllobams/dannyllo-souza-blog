using dbs.core.Data;
using dbs.domain.Repositories;
using dbs.infra.Context;
using Microsoft.EntityFrameworkCore;

namespace dbs.infra.Repositories
{
    public class PageViewRepository : IPageViewRepository
    {
        private readonly BlogContext _blogContext;
        public IUnitOfWork UnitOfWork => _blogContext;

        public PageViewRepository(BlogContext blogContext)
        {
            _blogContext = blogContext;
        }

        public async Task<int> GetTotalViews(CancellationToken cancellationToken = default)
        {
            const string sql = """
                SELECT COALESCE(SUM(pv."TotalViews"), 0) AS "Value"
                FROM "PageViews" pv
            """;

            return await _blogContext.Database
                .SqlQueryRaw<int>(sql)
                .SingleAsync(cancellationToken);
        }

        public async Task<int> GetTotalViews(Guid pageId, CancellationToken cancellationToken = default)
        {
            const string sql = """
                SELECT COALESCE(SUM(pv."TotalViews"), 0) AS "Value"
                FROM "PageViews" pv
                WHERE pv."PageId" = {0}
            """;

            return await _blogContext.Database
                .SqlQueryRaw<int>(sql, pageId)
                .SingleAsync(cancellationToken);
        }

        public async Task IncrementPageView(Guid pageId, CancellationToken cancellationToken = default)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            const string sql = """
                INSERT INTO "PageViews" (
                    "Id",
                    "PageId",
                    "Date",
                    "TotalViews",
                    "CreatedAt",
                    "UpdatedAt"
                )
                VALUES (
                    {0},
                    {1},
                    {2},
                    1,
                    (NOW() AT TIME ZONE 'UTC'),
                    (NOW() AT TIME ZONE 'UTC')
                )
                ON CONFLICT ("PageId", "Date")
                DO UPDATE SET
                    "TotalViews" = "PageViews"."TotalViews" + 1,
                    "UpdatedAt" = (NOW() AT TIME ZONE 'UTC')
            """;

            await _blogContext.Database.ExecuteSqlRawAsync(
                sql,
                new object[] { Guid.NewGuid(), pageId, today },
                cancellationToken
            );
        }
    }
}
