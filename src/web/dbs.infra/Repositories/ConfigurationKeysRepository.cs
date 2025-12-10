using dbs.core.Data;
using dbs.domain.Model;
using dbs.domain.Repositories;
using dbs.infra.Context;
using Microsoft.EntityFrameworkCore;

namespace dbs.infra.Repositories
{
    public class ConfigurationKeysRepository : IConfigurationKeysRepository
    {
        private  readonly BlogContext _blogContext;
        public IUnitOfWork UnitOfWork => _blogContext;


        public ConfigurationKeysRepository(BlogContext blogContext)
        {
            _blogContext = blogContext;
        }

        public async Task<ConfigurationKey?> GetByIdAsync(Guid id)
        {
            return await _blogContext.ConfigurationKeys.FindAsync(id);
        }

        public async Task<IEnumerable<ConfigurationKey>> GetAllAsync(int page, int pageSize)
        {
            return await _blogContext.ConfigurationKeys
                .AsNoTracking()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Guid> AddAsync(ConfigurationKey configurationKey)
        {
            await _blogContext.ConfigurationKeys.AddAsync(configurationKey);
            return configurationKey.Id;
        }

        public void Update(ConfigurationKey post)
        {
            _blogContext.ConfigurationKeys.Update(post);
        }

        public void Remove(ConfigurationKey post)
        {
            _blogContext.ConfigurationKeys.Remove(post);
        }
    }
}
