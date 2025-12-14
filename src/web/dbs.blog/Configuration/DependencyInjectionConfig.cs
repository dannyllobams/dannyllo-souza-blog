using Cortex.Mediator.DependencyInjection;
using dbs.core.Mediator;
using dbs.domain.Repositories;
using dbs.infra.Context;
using dbs.infra.Repositories;
using Microsoft.EntityFrameworkCore;

namespace dbs.blog.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BlogContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IMediatorHandler, MediatorHandler>();
            services.AddScoped<IPostsRepository, PostsRepository>();

            services.AddCortexMediator(
                configuration: configuration,
                handlerAssemblyMarkerTypes: new[] { typeof(Program) },
                configure: options =>
                {
                    options.AddDefaultBehaviors();
                }
            );
        }
    }
}
