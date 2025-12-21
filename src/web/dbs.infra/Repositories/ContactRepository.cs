using dbs.domain.Model;
using dbs.domain.Repositories;
using dbs.infra.Context;
using Microsoft.EntityFrameworkCore;

namespace dbs.infra.Repositories
{
    public class ContactRepository : RepositoryBase<Contact>, IContactRepository
    {
        public ContactRepository(BlogContext blogContext) : base(blogContext)
        {
        }

        public override async Task<IEnumerable<Contact>> GetAllAsync(int page, int pageSize)
        {
            return await _blogContext.Set<Contact>()
                .AsNoTracking()
                .OrderByDescending(c => c.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
