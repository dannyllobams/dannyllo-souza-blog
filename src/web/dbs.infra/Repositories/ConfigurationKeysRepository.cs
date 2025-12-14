using dbs.domain.Model;
using dbs.domain.Repositories;
using dbs.infra.Context;
using Microsoft.EntityFrameworkCore;

namespace dbs.infra.Repositories
{
    public class ConfigurationKeysRepository : RepositoryBase<ConfigurationKey>, IConfigurationKeysRepository
    {
        public ConfigurationKeysRepository(BlogContext blogContext) : base(blogContext)
        {
        }
    }
}
